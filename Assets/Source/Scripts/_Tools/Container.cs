using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools
{
    public abstract class Container : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        public Transform GetTransform()
        {
            return _transform;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Container))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}