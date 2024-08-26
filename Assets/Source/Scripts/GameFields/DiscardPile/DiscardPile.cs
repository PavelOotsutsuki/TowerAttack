using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Tools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.DiscardPiles
{
    public class DiscardPile
    {
        private const float CenterRotation = 90f;
        private readonly List<Seat> _seats =  new List<Seat>();

        private DiscardPileConfig _discardPileConfig;
        private float _maxCoordinateX;
        private float _maxCoordinateY;
        private float _minCoordinateX;
        private float _minCoordinateY;
        private SeatPool _discardPileSeatPool;

        private SignalBus _bus;

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

        private void OnDestroy()
        {
            _bus.Unsubscribe<DiscardCardsSignal>(OnDiscardCardsSignal);
        }

        private void OnDiscardCardsSignal(DiscardCardsSignal signal)
        {
            DiscardingCards(signal.Cards).ToUniTask();
        }

        private void SeatCard(Card card)
        {
            card.SetActiveInteraction(false);
            
            Seat discardPileSeat = GetSeat();
            discardPileSeat.SetCard(card, SideType.Back, _discardPileConfig.StartCardTranslateSpeed);
            
            //TODO: add seat removing
            _seats.Add(discardPileSeat);
        }

        private Seat GetSeat()
        {
            Seat discardPileSeat = _discardPileSeatPool.GetSeat();
            discardPileSeat.transform.SetParent(_discardPileConfig.RectTransform);
            discardPileSeat.SetLocalPositionValues(FindCardSeatPosition(), FindCardSeatRotation());
            return discardPileSeat;
        }

        private IEnumerator DiscardingCards(IEnumerable<Card> discardingCards)
        {
            foreach (Card card in discardingCards)
            {
                DiscardCardAnimation discardCardAnimation = new DiscardCardAnimation(_discardPileConfig.DiscardCardAnimationData, _discardPileConfig.RectTransform, card, SeatCard);
                discardCardAnimation.Play();
                yield return new WaitForSeconds(_discardPileConfig.DiscardDelay);
            }
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
