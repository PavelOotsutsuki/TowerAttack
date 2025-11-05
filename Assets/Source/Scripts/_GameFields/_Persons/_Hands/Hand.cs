using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Curses;
using GameFields.Persons.EffectHandlers.Slimes;
using GameFields.Seats;
using Tools.Utils;
using Tools.Utils.FillComponents;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour, ICardDragAndDropHandHandler, IHandBlockable, ICardView, IPersonObject, ICardsCounter,
        ICardFeatureRechangablePlace, ITransitable, ISlimeEffectWorker, ITurnSkipper, IAutomaticFillComponents
    {
        private const float StartRotation = 0;
        private const int EmptyIndex = -1;

        [SerializeField, Range(-1, 1)] private float _sortDirection;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        //[SerializeField] private float _handLength = 1175f;
        //[SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _returnInSeatDuration = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SideType _sideType;
        [SerializeField] private bool _isActiveInteraction;
        [SerializeField] private Transform _containerForDrag; //IPS
        [SerializeField] private Transform _containerForSeats; 

        private readonly List<Card> _handCursedCards = new List<Card>();
        private readonly List<Card> _handLuckyHorseshoeCards = new List<Card>();
        private List<Seat> _handSeats;
        private Seat _dragCardHandSeat;
        private Transform _dragCardParent; // IPS
        private int _handSeatIndex;
        private SeatPool _handSeatPool;
        private RechangeFeatureRuleController _ruleController;
        private IDrawnCardWatcher _turnDrawnCards;
        private CurseEffectHandler _curseEffectHandler;

        //private List<Card> _turnCardsFromDeck;
        private bool _isSlimeEffect = false;

        float ICardDragAndDropHandHandler.ReturnInSeatDuration => _returnInSeatDuration;

        public event Action OnSeatsCountChange;

        public int CountHandSeats => _handSeats.Count;
        public int CountCards => Cards.Count;
        public float HandLength => _rectTransform.rect.width;

        public IEnumerable<Card> AllCards => Cards;

        private List<Card> Cards
        {
            get
            {
                List<Card> cards = new List<Card>();

                foreach (Seat seat in _handSeats)
                {
                    if (seat.IsFill())
                        cards.Add(seat.Card);
                }

                if (_dragCardHandSeat != null)
                    if (_dragCardHandSeat.IsFill())
                        cards.Add(_dragCardHandSeat.Card);

                return cards;
            }
        }

        public bool CanSkip => CountCards == 0;
        public bool IsSlimeEffect => _isSlimeEffect;

        public void Init(SeatPool seatPool, RechangeFeatureRuleController ruleController, IDrawnCardWatcher turnDrawnCards,
            CurseEffectHandler curseEffectHandler)
        {
            _handSeats = new List<Seat>();
            _curseEffectHandler = curseEffectHandler;
            //_turnCardsFromDeck = new List<Card>();
            _handSeatIndex = EmptyIndex;
            _ruleController = ruleController;
            _turnDrawnCards = turnDrawnCards;

            _handSeatPool = seatPool;
        }

        public IEnumerable<IFeatureRechanger> GetRechangableCards()
        {
            return Cards;
        }

        bool ICardDragAndDropHandHandler.IsDraggable(Card card) => TryFindHandSeat(out Seat seat, card);

        void ICardDragAndDropHandHandler.OnCardDrag(Card card)
        {
            StartDragCard(card);
        }

        void ICardDragAndDropHandHandler.OnCardDrop()
        {
            UnblockCards();
            StartEndDragCard(false);
        }

        void ICardDragAndDropHandHandler.OnCardPlay()
        {
            //UnblockCards(); // Было раньше. Убрал тк, а нахер заблочивать???
            UnbindCurse(_dragCardHandSeat.Card);
            UnbindLuckyHorseshoe(_dragCardHandSeat.Card);

            BlockCards();
            UnbindDragableCard();
        }

        //void ICardDragAndDropHandHandler.OnCardAttack()
        //{
        //    BlockCards();
        //    UnbindDragableCard();
        //}

        void ICardDragAndDropHandHandler.OnCardReturnInHand(Card card)
        {
            card.SetActiveInteraction(_isActiveInteraction);
        }

        //void ITurnDrawCardWatcher.SetCard(Card card)
        //{
        //    _turnCardsFromDeck.Add(card);
        //}

        public void SeatCard(Card card)
        {
            //card.SetDragAndDropListener(this);

            _ruleController.TryRenameFeature(card);

            if (card.IsCurse)
            {
                _curseEffectHandler.Activate(card);
                _handCursedCards.Add(card);
            }

            if (card.IsLuckyHorseshoe)
            {
                _curseEffectHandler.SetDeactivateMode(true);
                _handLuckyHorseshoeCards.Add(card);
            }

            Seat handSeat = _handSeatPool.GetSeat();
            handSeat.transform.SetParent(_containerForSeats);
            handSeat.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _handSeats.Add(handSeat);
            handSeat.SetCard(card, _sideType, _returnInSeatDuration);
            //card.SetActiveInteraction(_isActiveInteraction);

            SetCardsInteraction();

            SortHandSeats();
        }

        //public bool TryGetRandomCard(out Card card)
        //{
        //    card = null;

        //    if (_isSlimeEffect)
        //    {
        //        return TryGetRandomCardFromDrawnCards(out card);
        //    }
        //    else
        //    {
        //        return TryGetDefaultRandomCard(out card);
        //    }
        //}

        public bool TryTakeAwayCard(Card card)
        {
            bool isFind = TryFindHandSeat(out Seat findedHandSeat, card);

            if (_dragCardHandSeat == findedHandSeat)
            {
                card.EndDrag();
                card.SetActiveInteraction(false);
                ResetDragOptions();
            }

            //if (_handSeats.Contains(findedHandSeat))
            //{
            //    _handSeats.Remove(findedHandSeat);
            //}
            //else if (_dragCardHandSeat == findedHandSeat)
            //{
            //    StartEndDragCard(true);
            //}
            //else
            //{
            //    throw new Exception("Не найден найденный HandSeat");
            //}
            UnbindCurse(card);
            UnbindLuckyHorseshoe(card);

            _handSeats.Remove(findedHandSeat);
            findedHandSeat.Reset();

            //_handSeatPool.ReturnInPool(findedHandSeat);

            SortHandSeats();

            return isFind;
        }

        //private Card UnbindLastCard()
        //{
        //    Seat lastSeat = _handSeats[_handSeats.Count - 1];
        //    Card gettedCard = lastSeat.Card;
        //    _handSeats.Remove(lastSeat);
        //    lastSeat.Reset();
        //    //_handSeatPool.ReturnInPool(lastSeat);

        //    SortHandSeats();

        //    return gettedCard;
        //}

        //public bool TryTakeAwayAllCards(out List<Card> cards)
        //{
        //    StartEndDragCard(true);

        //    if (_handSeats.Count <= 0)
        //    {
        //        cards = null;
        //        return false;
        //    }

        //    cards = new List<Card>();

        //    while (_handSeats.Count > 0)
        //    {
        //        cards.Add(UnbindLastCard());
        //    }

        //    return true;
        //}

        public void ForciblyBlock()
        {
            StartEndDragCard(true);
            BlockCards();
        }

        public void Unblock()
        {
            UnblockCards();
        }

        //public void OnStartTurn()
        //{
        //    _turnCardsFromDeck.Clear();
        //}

        //public void OnFinishTurn()
        //{
        //    _turnCardsFromDeck.Clear();

        //    //if (_countSlimeEffect > 0)
        //    //    _countSlimeEffect--;
        //}

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            IReadOnlyList<Card> cards = Cards;

            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            cards = Utils.Shuffle(cards);

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (existingIndices.Contains(cards[i].ViewData.Number) == false && exceptions.Contains(cards[i].ViewData.Number) == false)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                        break;
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (exceptions.Contains(cards[i].ViewData.Number) == false)
                        {
                            result.Add(cards[i]);
                            existingIndices.Add(cards[i].ViewData.Number);
                            break;
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
                throw new Exception("Ошибка вычисления чисел. Слишком мало карт!");

            return result;
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            int allCards = _handSeats.Where(s => s.IsFill() && exceptions?.Contains(s.Card.ViewData.Number) == false).Count();

            if (_dragCardHandSeat != null)
                if (_dragCardHandSeat.IsFill())
                    if (exceptions.Contains(_dragCardHandSeat.Card.ViewData.Number) == false)
                        allCards++;

            return allCards >= count;
        }

        public bool Contains(int number)
        {
            return Cards.Select(c => c.ViewData.Number).Contains(number);
        }

        void ISlimeEffectWorker.Activate()
        {
            _isSlimeEffect = true;
        }

        void ISlimeEffectWorker.Deactivate()
        {
            _isSlimeEffect = false;
        }

        private void BlockCards()
        {
            _isActiveInteraction = false;

            SetCardsInteraction();
        }

        private void UnbindDragableCard()
        {
            //_handSeatPool.ReturnInPool(_dragCardHandSeat);

            SortHandSeats();
            ResetDragOptions();
        }

        private void UnblockCards()
        {
            _isActiveInteraction = true;

            SetCardsInteraction();
        }

        private void StartEndDragCard(bool isForced)
        {
            if (_handSeatIndex == EmptyIndex)
                return;
            
            Card dragCard = _dragCardHandSeat.Card;

            if (isForced)
            {
                Debug.Log("Forcibly");

                dragCard.EndDrag();
                dragCard.SetActiveInteraction(false);
            }

            dragCard.ReadOnlyRectTransform.SetParent(_dragCardParent); // IPS
            _handSeats.Insert(_handSeatIndex, _dragCardHandSeat);

            SortHandSeats();
            ResetDragOptions();
        }

        private void StartDragCard(Card card)
        {
            if (TryFindHandSeat(out Seat handSeat, card))
            {
                _dragCardParent = card.transform.parent; // IPS
                card.ReadOnlyRectTransform.SetParent(_containerForDrag); // IPS
                _dragCardHandSeat = handSeat;

                _handSeatIndex = _handSeats.IndexOf(_dragCardHandSeat);
                _handSeats.Remove(_dragCardHandSeat);

                BlockCards();
                SortHandSeats();
            }
        }

        private void SetCardsInteraction()
        {
            //bool isActiveInteraction = _isActiveInteraction;

            //if (_countSlimeEffect > 0)
            //{
            //    isActiveInteraction = false;
            //}

            foreach (Seat seat in _handSeats)
            {
                Card card = seat.Card;

                if (_isSlimeEffect && _turnDrawnCards.DrawnCards.Contains(card) == false)
                {
                    card.SetActiveInteraction(false);
                }
                else
                {
                    card.SetActiveInteraction(_isActiveInteraction);
                }
            }
        }

        private void ResetDragOptions()
        {
            _handSeatIndex = EmptyIndex;
            _dragCardHandSeat = null;
        }

        private void UnbindCurse(Card card)
        {
            if (_handCursedCards.Contains(card))
            {
                _curseEffectHandler.EndEffect(card);
                _handCursedCards.Remove(card);
            }
        }

        private void UnbindLuckyHorseshoe(Card card)
        {
            if (_handLuckyHorseshoeCards.Contains(card))
            {
                _handLuckyHorseshoeCards.Remove(card);

                if (_handLuckyHorseshoeCards.Count == 0)
                {
                    _curseEffectHandler.SetDeactivateMode(false);
                }
            }
        }

        //UnbindLuckyHorseshoe

        //private bool TryGetDefaultRandomCard(out Card card)
        //{
        //    card = null;

        //    if (_handSeats.Count <= 0)
        //    {
        //        return false;
        //    }

        //    int randomIndex = Random.Range(0, _handSeats.Count);

        //    card = _handSeats[randomIndex].Card;

        //    return true;
        //}

        //private bool TryGetRandomCardFromDrawnCards(out Card card)
        //{
        //    card = null;

        //    if (_turnCardsFromDeck.Count <= 0)
        //    {
        //        return false;
        //    }

        //    int randomIndex = Random.Range(0, _turnCardsFromDeck.Count);

        //    card = _turnCardsFromDeck[randomIndex];

        //    return true;
        //}

        private bool TryFindHandSeat(out Seat findedHandSeat, Card card)
        {
            findedHandSeat = _handSeats.FirstOrDefault(s => s.Card == card);

            if (findedHandSeat == null)
            {
                if (_dragCardHandSeat.Card == card)
                    findedHandSeat = _dragCardHandSeat;
            }

            return findedHandSeat != null;
        }

        private void SortHandSeats()
        {
            //SetCardsInteraction();

            OnSeatsCountChange?.Invoke();

            if (_handSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (_handSeats.Count * _offsetX < HandLength / 2)
            {
                offsetX = _offsetX;
            }
            else
            {
                float xFactor = _offsetX * _handSeats.Count;
                float fullOffsetX = HandLength * xFactor / (xFactor + HandLength / 2);

                offsetX = fullOffsetX / _handSeats.Count;
            }

            offsetX *= -1 * _sortDirection;
            float startPositionX = _rectTransform.rect.width / 2f;

            for (int i = 0; i < _handSeats.Count; i++)
            {
                positionX = startPositionX + ((_handSeats.Count - 1) / 2f - i) * offsetX;
                Vector3 position = new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin);
                Vector3 rotation = new Vector3(0f, 0f, StartRotation);

                _handSeats[i].SetLocalPositionValues(position, rotation, _returnInSeatDuration);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Hand))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}