using UnityEngine;
using Cards;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using Tools;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        private Movement _seatMovement;

        public Card Card { get; private set; }
        public ReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Init()
        {
            _seatMovement = new Movement(_transform);
            ReadOnlyTransform = new ReadOnlyTransform(_transform);
            Reset();
        }

        public void Reset() => Card = null;

        public void SetCard(Card card, SideType sideType, float duration, float scaleFactor = 1f)
        {
            Card = card;

            Card.SetSide(sideType);
            Card.ReadOnlyRectTransform.SetParent(_transform);
            Movement cardMovement = Card.CardMovement;
            cardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, Card.DefaultScaleVector * scaleFactor);
        }

        public bool IsFill() => Card != null;

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(Seat))]
        public void DefineAllComponents()
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