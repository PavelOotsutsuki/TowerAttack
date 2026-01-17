using System;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.UI
{
    public class ConfirmableFocusableDisablableButton : ConfirmableFocusableButton
    {
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Color _disableColor;

        private bool _isDisable;

        public bool IsDisable => _isDisable;

        public override void Init(IFocusWatcher focusWatcher, Action onEnterClick)
        {
            base.Init(focusWatcher, onEnterClick);

            _isDisable = false;

            if (onEnterClick == null)
                SetDisableView();
        }

        public void SetDisableView()
        {
            CanvasGroup.blocksRaycasts = false;
            _image.color = _disableColor;
            _isDisable = true;
        }

        public void SetUndisableView()
        {
            CanvasGroup.blocksRaycasts = true;
            ImageChanger.OnActivate();
            _isDisable = false;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ConfirmableFocusableDisablableButton))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineImage(),
                DefineCanvasGroup()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineImage))]
        private ComponentAttachInfo DefineImage()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _image, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}