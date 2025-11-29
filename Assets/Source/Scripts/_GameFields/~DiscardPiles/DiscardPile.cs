using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Views;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Seats;
using GameFields.Signals;
using Tools.Utils;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.DiscardPiles
{
    public class DiscardPile: ICardView, ITransitable
    {
        private const float CenterRotation = 90f;

        private readonly List<Seat> _seats =  new List<Seat>();
        private readonly DiscardPileConfig _discardPileConfig;
        private readonly float _maxCoordinateX;
        private readonly float _maxCoordinateY;
        private readonly float _minCoordinateX;
        private readonly float _minCoordinateY;
        private readonly SeatPool _discardPileSeatPool;

        private readonly SignalBus _bus;

        public IEnumerable<Card> AllCards => _seats.Select(s => s.Card);

        public DiscardPile(SeatPool seatPool, SignalBus bus, DiscardPileConfig discardPileConfig)
        {
            _discardPileConfig = discardPileConfig;
            _maxCoordinateX = _discardPileConfig.RectTransform.rect.width / 2f;
            _maxCoordinateY = _discardPileConfig.RectTransform.rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
            _discardPileSeatPool = seatPool;
            _bus = bus;
            _bus.Subscribe<DiscardCardsSignal>(OnDiscardCardsSignal);
        }

        ~DiscardPile()
        {
            _bus.Unsubscribe<DiscardCardsSignal>(OnDiscardCardsSignal);
        }

        private void OnDiscardCardsSignal(DiscardCardsSignal signal)
        {
            DiscardingCards(signal.Card).ToUniTask();
        }

        public void SeatCard(Card card, int index = -1)
        {
            if (index < 0 || index > _seats.Count)
                index = _seats.Count;

            card.SetActiveInteraction(false);

            Seat discardPileSeat = GetSeat();
            discardPileSeat.SetCard(card, SideType.Back, _discardPileConfig.StartCardTranslateSpeed);
            //TODO: add seat removing
            _seats.Insert(index, discardPileSeat);
        }

        public bool TryTakeAwayCard(Card card)
        {
            Seat seat = _seats.Where(s => s.Card == card).FirstOrDefault();

            if (seat == null)
                return false;

            _seats.Remove(seat);
            seat.Reset();

            return true;
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            IReadOnlyList<Card> cards = Utils.Shuffle(_seats.Select(s => s.Card));

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (existingIndices.Contains(cards[i].ViewData.Number) == false && exceptions.Contains(cards[i].ViewData.Number) == false)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (exceptions.Contains(cards[i].ViewData.Number) == false)
                        {
                            result.Add(cards[i]);
                            existingIndices.Add(cards[i].ViewData.Number);
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
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

            //for (int i = 0; i < count; i++)
            //{
            //    int randomIndex = Random.Range(0, _seats.Count);

            //    while (existingIndices.Contains(randomIndex) || exceptions.Contains(randomIndex))
            //    {
            //        randomIndex = Random.Range(0, _seats.Count);
            //    }

            //    result.Add(_seats[randomIndex].Card);
            //}

            return result;
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            return _seats.Where(s => exceptions.Contains(s.Card.ViewData.Number) == false).Count() >= count; 
        }

        public bool Contains(int number)
        {
            return _seats.Select(s => s.Card.ViewData.Number).Contains(number);
        }

        public int IndexOf(Card card)
        {
            return _seats.IndexOf(_seats.Where(s => s.Card == card).First());
        }

        private Seat GetSeat()
        {
            Seat discardPileSeat = _discardPileSeatPool.GetSeat();
            discardPileSeat.ReadOnlyTransform.SetParent(_discardPileConfig.RectTransform);
            discardPileSeat.SetLocalPositionValues(FindCardSeatPosition(), FindCardSeatRotation());
            return discardPileSeat;
        }

        //private IEnumerator DiscardingCards(IEnumerable<Card> discardingCards)
        //{
        //    foreach (Card card in discardingCards)
        //    {
        //        DiscardCardAnimation discardCardAnimation = new DiscardCardAnimation(_discardPileConfig.DiscardCardAnimationData, _discardPileConfig.RectTransform, card, SeatCard);
        //        discardCardAnimation.Play();
        //        yield return new WaitForSeconds(_discardPileConfig.DiscardDelay);
        //    }
        //}

        private IEnumerator DiscardingCards(Card discardingCard)
        {
            DiscardCardAnimation discardCardAnimation = new DiscardCardAnimation(_discardPileConfig.DiscardCardAnimationData, _discardPileConfig.RectTransform, discardingCard, (card) => SeatCard(card, -1));
            discardCardAnimation.Play();
            yield return new WaitForSeconds(_discardPileConfig.DiscardDelay);
        }

        private Vector3 FindCardSeatPosition()
        {
            float xCoordinate = Random.Range(_minCoordinateX, _maxCoordinateX);
            float yCoordinate = Random.Range(_minCoordinateY, _maxCoordinateY);

            return new Vector3(xCoordinate, yCoordinate, 0f);
        }

        private Vector3 FindCardSeatRotation()
        {
            float zRotation = Random.Range(CenterRotation - _discardPileConfig.CardRotationOffset, CenterRotation + _discardPileConfig.CardRotationOffset);

            return new Vector3(0f, 0f, zRotation);
        }
    }
}