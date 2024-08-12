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
            _card.Transform.localPosition = Vector2.zero;
        }

        internal void ResetCard() => _card = null;

        internal bool CompareSeatable(Card card) => card == _card;

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

