using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Movement _seatMovement;

        public Card Card { get; private set; }

        public virtual void Init()
        {
            _seatMovement = new Movement(_transform);
            Reset();
        }

        public void Reset() => Card = null;

        public void SetCard(Card card, SideType sideType, float duration, float scaleFactor = 1f)
        {
            Card = card;

            Card.SetSide(sideType);
            Card.transform.SetParent(_transform);
            CardMovement cardMovement = Card.CardMovement;
            cardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, Card.DefaultScaleVector * scaleFactor);
        }

        public bool IsCardEqual(Card card) => Card == card;

        public bool IsFill() => Card != null;

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        #region AutomaticFillComponents

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

        #endregion 
    }
}
