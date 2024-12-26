using UnityEngine;
using Cards;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using System.Collections.Generic;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;

        private Card _card;

        internal bool IsEmpty => _card == null;

        internal void SetCard(Card card)
        {
            _card = card;
            _card.ReadOnlyRectTransform.SetParent(_rectTransform);

            Movement cardMovement = _card.CardMovement;

            cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
        }

        internal void Reset() => _card = null;

        internal bool IsCardEqual(Card card) => card == _card;

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TableSeat))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}