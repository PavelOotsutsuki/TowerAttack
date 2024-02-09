using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Card _card;
        private Movement _handSeatMovement;

        public void Init()
        {
            _handSeatMovement = new Movement(_transform);
        }

        public Card GetCard()
        {
            return _card;
        }

        public void SetCard(Card card, bool isFrontCardSide, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, isFrontCardSide, duration);
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

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration)
        {
            _handSeatMovement.MoveLocalSmoothly(position, rotation, duration);
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
