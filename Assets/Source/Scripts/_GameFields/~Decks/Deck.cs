using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools.Utils.FillComponents;
using Tools;
using Tools.Utils.Movements;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Tools.Utils;
using GameFields.Seats;
using GameFields.DiscardPiles;
using Cards.Views;

namespace GameFields.Decks
{
    public class Deck : MonoBehaviour, IAutomaticFillComponents, IDeckTake, IDeckView, ITransitable, ICardsCounter//, IDeckSeatable
    {
        [SerializeField] private DeckCardContainer _cardContainer;
        [SerializeField] private DeckCardBackViewer _cardBackViewer;
        [SerializeField] private DeckHelper _deckHelper;
        [SerializeField] private int _countCardsInGroup = 10;

        private readonly float _startCardAddPositionX = 0f;
        private readonly float _startCardAddPositionY = 0f;
        private List<Seat> _seats = new List<Seat>();

        private SeatPool _deckSeatPool;

        public event Action OnSeatsCountChange;

        public int CountCards => _seats.Count;

        public IEnumerable<Card> AllCards => _seats.Select(s => s.Card);

        public void Init(SeatPool seatPool, IEnumerable<Card> cards)
        {
            _deckSeatPool = seatPool;
            _cardBackViewer.Init(_startCardAddPositionX, _startCardAddPositionY);
            _deckHelper.Init();

            foreach (Card card in cards)
            {
                SeatCard(card, _seats.Count);
                //Seat deckSeat = GetSeat();
                //deckSeat.SetCard(card, SideType.Back, 0f);

                //_seats.Add(deckSeat);



                //BindCard(card.ReadOnlyRectTransform, card.CardMovement);
            }

            ShuffleCards();
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            return _seats.Where(c => exceptions.Contains(c.Card.ViewData.Number) == false).Count() >= count;
        }

        public bool Contains(int number)
        {
            return _seats.Select(c => c.Card.ViewData.Number).Contains(number);
        }

        public void SeatCard(Card card, int index = -1)
        {
            bool isShuffle = index == -1;

            if (index < 0 || index > _seats.Count)
                index = _seats.Count;

            Seat deckSeat = GetSeat();
            deckSeat.SetCard(card, SideType.Back, 0.5f);

            //if (AllCards.Count() % _countCardsInGroup == 1)
            //{
            //    _cardBackViewer.Add();
            //}
            CheckBackViewer();

            _seats.Insert(index, deckSeat);
            OnSeatsCountChange?.Invoke();

            if (isShuffle)
                ShuffleCards();
        }

        private void CheckBackViewer()
        {
            int countNeed = (AllCards.Count() + (_countCardsInGroup - 1)) / _countCardsInGroup;

            while (countNeed > _cardBackViewer.Count)
            {
                _cardBackViewer.Add();
            }

            while (countNeed < _cardBackViewer.Count)
            {
                _cardBackViewer.Remove();
            }
        }

        //public void SeatCardWithoutShuffle(Card card)
        //{
        //    int position = Random.Range(0, _cards.Count);
        //    _cards.Insert(position, card);

        //    OnSeatsCountChange?.Invoke();

        //    BindCard(card.ReadOnlyRectTransform, card.CardMovement);
        //}

        public Card TakeTopCard()
        {
            return TakeCardByIndex(_seats.Count - 1);
        }

        public int IndexOf(Card card)
        {
            Seat seat = _seats.Where(s => s.Card == card).FirstOrDefault();

            if (seat == null)
                throw new Exception("Заебал. Как ты вообще сюда попал. Херли ты берешь карту из деки если её тут нет");

            return _seats.IndexOf(seat);
        }

        public bool TryTakeAwayCard(Card card)
        {
            if (IsHasCards(1) == false)
                return false;

            RemoveCard(card);

            return true;
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            IReadOnlyList<Seat> seats = Utils.Shuffle(_seats);

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < seats.Count; i++)
                {
                    if (existingIndices.Contains(seats[i].Card.ViewData.Number) == false && exceptions.Contains(seats[i].Card.ViewData.Number) == false)
                    {
                        result.Add(seats[i].Card);
                        existingIndices.Add(seats[i].Card.ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < seats.Count; i++)
                    {
                        if (exceptions.Contains(seats[i].Card.ViewData.Number) == false)
                        {
                            result.Add(seats[i].Card);
                            existingIndices.Add(seats[i].Card.ViewData.Number);
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < seats.Count; i++)
                    {
                        result.Add(seats[i].Card);
                        existingIndices.Add(seats[i].Card.ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
                throw new Exception("Ошибка вычисления чисел. Слишком мало карт!");

            return result;
        }

        public Card ViewCardFromTopDeck(int index = 0)
        {
            return _seats[_seats.Count - 1 - index].Card;
        }

        public Card ViewCardFromEndDeck(int index = 0)
        {
            return _seats[index].Card;
        }

        private Card TakeCardByIndex(int index)
        {
            //Seat seat = _seats[index];
            Card card = _seats[index].Card;

            RemoveCard(card);

            return card;
        }

        private void ShuffleCards()
        {
            _seats = Utils.Shuffle(_seats);
            //List<Card> shuffleCards = new List<Card>();

            //while (_cards.Count > 0)
            //{
            //    Card card = _cards[Random.Range(0, _cards.Count)];
            //    shuffleCards.Add(card);
            //    _cards.Remove(card);
            //}

            //_cards = shuffleCards;
        }

        private void RemoveCard(Card card)
        {
            Seat seat = _seats.Where(s => s.Card == card).FirstOrDefault();

            if (seat == null)
                throw new Exception("Пытаетесь удалить карту которой нет в Seat-а");

            _seats.Remove(seat);

            OnSeatsCountChange?.Invoke();

            //if (AllCards.Count() % _countCardsInGroup == 0)
            //{
            //    _cardBackViewer.Remove();
            //}

            CheckBackViewer();
        }

        //private void BindCard(ReadOnlyTransform cardTransform, Movement cardMovement)
        //{
        //    cardTransform.SetParent(_cardContainer.GetTransform());

        //    Vector2 cardAddPosition = new Vector2(_startCardAddPositionX, _startCardAddPositionY);

        //    cardMovement.MoveLocalInstantly(cardAddPosition, cardTransform.GetRotationVector());

        //    if (_seats.Count % _countCardsInGroup == 1)
        //    {
        //        _cardBackViewer.Add();
        //    }
        //}

        private Seat GetSeat()
        {
            Seat discardPileSeat = _deckSeatPool.GetSeat();
            discardPileSeat.ReadOnlyTransform.SetParent(_cardContainer.GetTransform());
            discardPileSeat.SetLocalPositionValues(new Vector2(_startCardAddPositionX, _startCardAddPositionY), Quaternion.identity.eulerAngles);
            return discardPileSeat;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Deck))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDeckCardContainer(),
                DeckCardBackViewer(),
                DeckCardDeckHelper()
            };

            return list;
        }

        [ContextMenu(nameof(DefineDeckCardContainer))]
        private ComponentAttachInfo DefineDeckCardContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardContainer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DeckCardBackViewer))]
        private ComponentAttachInfo DeckCardBackViewer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardBackViewer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DeckCardDeckHelper))]
        private ComponentAttachInfo DeckCardDeckHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _deckHelper, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}