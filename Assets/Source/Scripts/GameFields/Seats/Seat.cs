using UnityEngine;
using Cards;
using Tools;
using System;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Movement _seatMovement;

        public Card Card { get; private set; }

        public void Init()
        {
            _seatMovement = new Movement(_transform);
            Reset();
        }

        public void Reset()
        {
            Card = null;
        }

        public void SetCard(Card card, SideType sideType, float duration)
        {
            Card = card;
            Card.SetSide(sideType);
            Card.BindDragAndDropContainer(_transform);

            Movement cardMovement = new(Card.Transform);
            cardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, Card.DefaultScaleVector);
        }

        public bool IsCardEqual(Card card)
        {
            return Card == card;
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
            return Card != null;
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
