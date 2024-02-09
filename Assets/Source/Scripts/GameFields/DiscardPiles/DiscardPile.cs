using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    public class DiscardPile : MonoBehaviour
    {
        private const float MinCoordinateX = 0f;
        private const float MinCoordinateY = 0f;
        private const float CenterRotation = 90f;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SeatPool<DiscardPileSeat> _discardPileSeatPool;
        [SerializeField] private bool _isFrontCardSide;
        [SerializeField] private float _cardRotationOffset = 45f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;

        private float _maxCoordinateX;
        private float _maxCoordinateY;
        private List<DiscardPileSeat> _seats;

        public void Init()
        {
            _maxCoordinateX = _rectTransform.rect.width;
            _maxCoordinateY = _rectTransform.rect.height;
            _seats = new List<DiscardPileSeat>();
            _discardPileSeatPool.Init();
        }

        public void AddCard(Card card)
        {
            if (_discardPileSeatPool.TryGetHandSeat(out DiscardPileSeat discardPileSeat))
            {
                _seats.Add(discardPileSeat);
                discardPileSeat.SetCard(card, _isFrontCardSide, _startCardTranslateSpeed);
            }
        }

        //public void RemoveCard()
        //{
        //    _discardPileSeatPool.ReturnInPool(_dragCardHandSeat);

        //    SortHandSeats();
        //    ResetDragOptions();
        //}

        private Vector3 FindCardSeatPosition()
        {
            float xCoordinate = Random.Range(MinCoordinateX, _maxCoordinateX);
            float yCoordinate = Random.Range(MinCoordinateY, _maxCoordinateY);

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
            DefineDiscardPileSeatPool();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineDiscardPileSeatPool))]
        private void DefineDiscardPileSeatPool()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPileSeatPool, ComponentLocationTypes.InThis);
        }
    }
}
