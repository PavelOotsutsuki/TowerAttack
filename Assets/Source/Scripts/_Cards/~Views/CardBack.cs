using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Views
{
    internal class CardBack : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;

        private Vector2 _cardSize;

        public Vector2 CardSize => _cardSize;

        public void Init(Vector2 size)
        {
            ReadOnlyRectTransform RORTransform = new ReadOnlyRectTransform(_rectTransform);

            _cardSize = size;

            RORTransform.SetSize(size);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
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