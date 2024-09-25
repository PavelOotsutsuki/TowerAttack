using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionSeat : MonoBehaviour
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

        public void SetCard(Card card, float duration, float scaleFactor = 1f)
        {
            Card = card;

            //Card.StartSelection();
            Card.SetSide(SideType.Front);
            Card.transform.SetParent(_transform);
            CardMovement cardMovement = Card.CardMovement;
            cardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, Card.DefaultScaleVector * scaleFactor);
        }

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
