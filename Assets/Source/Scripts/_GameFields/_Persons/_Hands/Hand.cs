using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Seats;
using Tools.Utils.FillComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour, ICardDragAndDropHandHandler, IHandBlockable, IReadOnlyHand, ITurnDrawCardWatcher,
        ICardView, IPersonObject, IAutomaticFillComponents
    {
        private const float StartRotation = 0;
        private const int EmptyIndex = -1;

        [SerializeField, Range(-1, 1)] private float _sortDirection;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        [SerializeField] private float _handLength = 1175f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _returnInSeatDuration = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SideType _sideType;
        [SerializeField] private bool _isActiveInteraction;
        [SerializeField] private Transform _container; //IPS

        private List<Seat> _handSeats;
        private Seat _dragCardHandSeat;
        private Transform _dragCardParent; // IPS
        private int _handSeatIndex;
        private SeatPool _handSeatPool;

        private List<Card> _turnCardsFromDeck;
        private int _countSlimeEffect = 0;

        float ICardDragAndDropHandHandler.ReturnInSeatDuration => _returnInSeatDuration;

        public int CountCards => _handSeats.Count;
        public bool IsSlimeEffectCountZero => _turnCardsFromDeck.Count == 0 && _countSlimeEffect > 0;

        public void Init(SeatPool seatPool)
        {
            _handSeats = new List<Seat>();
            _turnCardsFromDeck = new List<Card>();
            _handSeatIndex = EmptyIndex;

            _handSeatPool = seatPool;
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
            UnblockCards();
            UnbindDragableCard();
        }

        void ICardDragAndDropHandHandler.OnCardAttack()
        {
            BlockCards();
            UnbindDragableCard();
        }

        void ICardDragAndDropHandHandler.OnCardReturnInHand(Card card)
        {
            card.SetActiveInteraction(_isActiveInteraction);
        }

        void ITurnDrawCardWatcher.SetCard(Card card)
        {
            _turnCardsFromDeck.Add(card);
        }

        public void AddCard(Card card)
        {
            //card.SetDragAndDropListener(this);

            Seat handSeat = _handSeatPool.GetSeat();
            handSeat.transform.SetParent(_rectTransform);
            handSeat.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _handSeats.Add(handSeat);
            handSeat.SetCard(card, _sideType, _returnInSeatDuration);
            card.SetActiveInteraction(_isActiveInteraction);

            SortHandSeats();
        }

        public bool TryGetCard(out Card card)
        {
            card = null;

            if (_countSlimeEffect > 0)
            {
                return TryGetRandomCardFromDrawnCards(out card);
            }
            else
            {
                return TryGetRandomCard(out card);
            }
        }

        public bool TryGetCard(Card card)
        {
            bool isFind = TryFindHandSeat(out Seat findedHandSeat, card);

            _handSeats.Remove(findedHandSeat);
            findedHandSeat.Reset();

            SortHandSeats();

            return isFind;
        }

        public Card UnbindLastCard()
        {
            Seat lastSeat = _handSeats[_handSeats.Count - 1];
            Card gettedCard = lastSeat.Card;
            _handSeats.Remove(lastSeat);
            lastSeat.Reset();

            SortHandSeats();

            return gettedCard;
        }

        public bool TryGetAllCards(out List<Card> cards)
        {
            StartEndDragCard(true);

            if (_handSeats.Count <= 0)
            {
                cards = null;
                return false;
            }

            cards = new List<Card>();

            while (_handSeats.Count > 0)
            {
                cards.Add(UnbindLastCard());
            }

            return true;
        }

        public void ForciblyBlock()
        {
            StartEndDragCard(true);
            BlockCards();
        }

        public void Unblock()
        {
            UnblockCards();
        }

        public void ActivateSlimeEffect(int countTurns)
        {
            _countSlimeEffect = countTurns;
        }

        public void OnStartTurn()
        {
            _turnCardsFromDeck.Clear();
        }

        public void OnFinishTurn()
        {
            _turnCardsFromDeck.Clear();

            if (_countSlimeEffect > 0)
                _countSlimeEffect--;
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
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

            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, cards.Count);

                while (existingIndices.Contains(randomIndex) || exceptions.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, cards.Count);
                }

                result.Add(cards[randomIndex]);
            }

            return result;
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            int allCards = _handSeats.Where(s => s.IsFill() && exceptions?.Contains(s.Card.ViewConfig.Number) == false).Count();

            if (_dragCardHandSeat != null)
                if (_dragCardHandSeat.IsFill())
                    if (exceptions.Contains(_dragCardHandSeat.Card.ViewConfig.Number) == false)
                        allCards++;

            return allCards >= count;
        }

        private void BlockCards()
        {
            _isActiveInteraction = false;

            SetCardsInteraction();
        }

        private void UnbindDragableCard()
        {
            _handSeatPool.ReturnInPool(_dragCardHandSeat);

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
                card.ReadOnlyRectTransform.SetParent(_container); // IPS
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

                if (_countSlimeEffect > 0 && _turnCardsFromDeck.Contains(card) == false)
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

        private bool TryGetRandomCard(out Card card)
        {
            card = null;

            if (_handSeats.Count <= 0)
            {
                return false;
            }

            int randomIndex = Random.Range(0, _handSeats.Count);

            card = _handSeats[randomIndex].Card;

            return true;
        }

        private bool TryGetRandomCardFromDrawnCards(out Card card)
        {
            card = null;

            if (_turnCardsFromDeck.Count <= 0)
            {
                return false;
            }

            int randomIndex = Random.Range(0, _turnCardsFromDeck.Count);

            card = _turnCardsFromDeck[randomIndex];

            return true;
        }

        private bool TryFindHandSeat(out Seat findedHandSeat, Card card)
        {
            findedHandSeat = _handSeats.FirstOrDefault(seat => seat.Card == card);

            return findedHandSeat != null;
        }

        private void SortHandSeats()
        {
            if (_handSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (_handSeats.Count * _offsetX < _handLength / 2)
            {
                offsetX = _offsetX;
            }
            else
            {
                float xFactor = _offsetX * _handSeats.Count;
                float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

                offsetX = fullOffsetX / _handSeats.Count;
            }

            offsetX *= -1 * _sortDirection;

            for (int i = 0; i < _handSeats.Count; i++)
            {
                positionX = _startPositionX + ((_handSeats.Count - 1) / 2f - i) * offsetX;
                Vector3 positon = new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin);
                Vector3 rotation = new Vector3(0f, 0f, StartRotation);

                _handSeats[i].SetLocalPositionValues(positon, rotation, _returnInSeatDuration);
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