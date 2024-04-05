using UnityEngine;
using Cards;
using Tools;
using System;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Card _card;
        private Movement _seatMovement;

        public void Init()
        {
            _seatMovement = new Movement(_transform);
            Reset();
        }

        public void Reset()
        {
            _card = null;
        }

        public Card GetCard()
        {
            return _card;
        }

        public void SetCard(Card card, bool isFrontSeatableObjectSide, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, isFrontSeatableObjectSide, duration);
        }

        public bool IsCardEqual(Card card)
        {
            return _card == card;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Disactivate()
        {
            gameObject.SetActive(false);
        }

        public bool IsFill()
        {
            return _card != null;
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                _seatMovement.MoveLocalInstantly(position, rotation);
            }
            else
            {
                _seatMovement.MoveLocalSmoothly(position, rotation, duration);
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
