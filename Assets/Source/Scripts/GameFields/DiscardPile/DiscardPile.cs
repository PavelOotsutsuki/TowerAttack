using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.Seats;
using Tools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Discarding
{
    public class DiscardPile : MonoBehaviour
    {
        private const float CenterRotation = 90f;
        private readonly List<Seat> _seats =  new();

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _cardRotationOffset = 30f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private float _discardDelay = 0.5f;
        [SerializeField] private DiscardCardAnimationData _discardCardAnimationData;

        private float _maxCoordinateX;
        private float _maxCoordinateY;
        private float _minCoordinateX;
        private float _minCoordinateY;
        private SeatPool _discardPileSeatPool;

        private SignalBus _bus;

        [Inject]
        private void Construct(SignalBus bus)
        {
            _bus = bus;
            
            _bus.Subscribe<DiscardCardsSignal>(OnDiscardCardsSignal);
        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<DiscardCardsSignal>(OnDiscardCardsSignal);
        }

        private void OnDiscardCardsSignal(DiscardCardsSignal signal)
        {
            DiscardCards(signal.Cards);
        }

        public void Init(SeatPool seatPool)
        {
            _maxCoordinateX = _rectTransform.rect.width / 2f;
            _maxCoordinateY = _rectTransform.rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
            _discardPileSeatPool = seatPool;
        }

        private void DiscardCards(List<Card> cards)
        {
            DiscardingCards(cards).ToUniTask();
        }

        private void SeatCard(Card card)
        {
            Seat discardPileSeat = _discardPileSeatPool.GetHandSeat();
            discardPileSeat.transform.SetParent(_rectTransform);
            card.SetActiveInteraction(false);
            _seats.Add(discardPileSeat);
            discardPileSeat.SetLocalPositionValues(FindCardSeatPosition(), FindCardSeatRotation());
            discardPileSeat.SetCard(card, SideType.Back, _startCardTranslateSpeed);
        }

        private IEnumerator DiscardingCards(List<Card> discardingCards)
        {
            foreach (Card card in discardingCards)
            {
                DiscardCardAnimation discardCardAnimation = new(_discardCardAnimationData, _rectTransform, card, SeatCard);
                discardCardAnimation.Play();
                yield return new WaitForSeconds(_discardDelay);
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
            float zRotation = Random.Range(CenterRotation - _cardRotationOffset, CenterRotation + _cardRotationOffset);

            return new Vector3(0f, 0f, zRotation);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
            DefineSeatPool();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineSeatPool))]
        private void DefineSeatPool()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPileSeatPool, ComponentLocationTypes.InThis);
        }
    }
}
