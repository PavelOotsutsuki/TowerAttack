using UnityEngine;
using Tools;
using Cards;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private Card _card;

        internal bool IsEmpty => _card == null;

        internal void SetCard(Card card)
        {
            _card = card;
            _card.Transform.SetParent(_rectTransform);
            //_card.Transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
            CardMovement cardMovement = _card.CardMovement;
            //cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
            cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
        }

        internal void Reset() => _card = null;

        internal bool IsCardEqual(Card card) => card == _card;

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}

