using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.LightControls
{
    public abstract class LightableObject : MonoBehaviour, IViewable, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private LightFrame _lightFrame;
        [SerializeField] private LightObjectsParent _lightObjectsParent;

        private Transform _defaultParent;
        private Transform _lightParent;

        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _defaultParent = _transform.parent;
            _lightParent = _lightObjectsParent.GetTransform();

            _lightFrame.Init();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _lightFrame.Show();
            _transform.SetParent(_lightParent);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _lightFrame.Hide();
            _transform.SetParent(_defaultParent);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LightableObject))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform(),
                DefineLightFrame(),
                DefineLightObjectsParent()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineLightFrame))]
        private ComponentAttachInfo DefineLightFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightFrame, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLightObjectsParent))]
        private ComponentAttachInfo DefineLightObjectsParent()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightObjectsParent, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}