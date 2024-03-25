using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] Transform _transform;
        [SerializeField] Transform _childrenTransform;

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
            DefineChildrenTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineChildrenTransform))]
        private void DefineChildrenTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _childrenTransform, ComponentLocationTypes.InChildren);
        }
    }
}
