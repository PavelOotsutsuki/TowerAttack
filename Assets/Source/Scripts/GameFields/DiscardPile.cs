using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class DiscardPile : MonoBehaviour, ICardDropSeatPlaceImitation
    {
        private const float CenterRotation = 90f;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SeatPool _discardPileSeatPool;
        [SerializeField] private bool _isFrontCardSide;
        [SerializeField] private float _cardRotationOffset = 30f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;

        private float _maxCoordinateX;
        private float _maxCoordinateY;
        private float _minCoordinateX;
        private float _minCoordinateY;
        private List<Seat> _seats;

        public void Init()
        {
            _maxCoordinateX = _rectTransform.rect.width / 2f;
            _maxCoordinateY = _rectTransform.rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
            _seats = new List<Seat>();
            _discardPileSeatPool.Init();
        }

        //public void AddCard(Card card)
        //{
        //    if (_discardPileSeatPool.TryGetHandSeat(out Seat discardPileSeat))
        //    {
        //        _seats.Add(discardPileSeat);
        //        discardPileSeat.SetLocalPositionValues(FindCardSeatPosition(), FindCardSeatRotation());
        //        discardPileSeat.SetCard(card, _isFrontCardSide, _startCardTranslateSpeed);
        //    }
        //}

        public bool TrySeatCard(ISeatable seatableObject)
        {
            if (_discardPileSeatPool.TryGetHandSeat(out Seat discardPileSeat))
            {
                _seats.Add(discardPileSeat);
                discardPileSeat.SetLocalPositionValues(FindCardSeatPosition(), FindCardSeatRotation());
                discardPileSeat.SetCard(card, _isFrontCardSide, _startCardTranslateSpeed);
            }
        }

        public Vector3 GetCentralСoordinates()
        {
            throw new System.NotImplementedException();
        }

        //public void RemoveCard()
        //{
        //    _discardPileSeatPool.ReturnInPool(_dragCardHandSeat);

        //    SortHandSeats();
        //    ResetDragOptions();
        //}

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
