using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.UI.UIHelpers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields
{
    public abstract class MenuActivateButton : ConfirmableButton, IWorkable
    {
        [SerializeField] private UIHelper _UIHelper;

        private IWorkable _menu;

        protected void Init(IWorkable menu, UIHelperDescription UIHelperDescription)
        {
            base.Init();

            _menu = menu;
            _UIHelper.Init(UIHelperDescription, () => GetHelperText());

            BaseDeactivate();
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            base.BaseActivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            base.BaseDeactivate();
        }

        protected abstract string GetHelperText();

        protected override void OnEnterClick()
        {
            if (_menu.IsActive == false)
            {
                _menu.Activate();
            }
            else
            {
                _menu.Deactivate();
            }

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(MenuActivateButton))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineUIHelper()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineUIHelper))]
        private ComponentAttachInfo DefineUIHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _UIHelper, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}