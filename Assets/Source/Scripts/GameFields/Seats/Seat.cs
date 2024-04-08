using UnityEngine;
using Cards;
using Tools;
using System;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private ISeatable _seatableObject;
        private Movement _seatMovement;

        public void Init()
        {
            _seatMovement = new Movement(_transform);
            Reset();
        }

        public void Reset()
        {
            _seatableObject = null;
        }

        public ISeatable GetCard()
        {
            return _seatableObject;
        }

        public void SetCard(ISeatable seatableObject, bool isFrontSeatableObjectSide, float duration)
        {
            _seatableObject = seatableObject;
            _seatableObject.BindSeat(_transform, isFrontSeatableObjectSide, duration);
        }

        public bool IsCardEqual(ISeatable seatableObject)
        {
            return _seatableObject == seatableObject;
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
            return _seatableObject != null;
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
