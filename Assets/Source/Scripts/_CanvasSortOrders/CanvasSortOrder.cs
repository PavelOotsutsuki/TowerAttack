using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace CanvasSortOrders
{
    [RequireComponent(typeof(Canvas))]
    public abstract class CanvasSortOrder : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Canvas _canvas;

        private int _defaultIndex;

        public void Init()
        {
            _defaultIndex = _canvas.sortingOrder;
        }

        public void SetDefaultIndex()
        {
            _canvas.sortingOrder = _defaultIndex;
        }

        public void SetSortIndex(int sortIndex)
        {
            _canvas.sortingOrder = sortIndex;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvas()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvas))]
        private ComponentAttachInfo DefineCanvas()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvas, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}