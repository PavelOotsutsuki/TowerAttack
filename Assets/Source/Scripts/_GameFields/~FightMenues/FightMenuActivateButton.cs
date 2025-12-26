using System;
using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.UI.UIHelpers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenuActivateButton : ConfirmableButton
    {
        [SerializeField] private UIHelper _UIHelper;

        private IActivatable _fightMenu;

        public void Init(IActivatable fightMenu, UIHelperDescription UIHelperDescription)
        {
            base.Init();

            _fightMenu = fightMenu;
            _UIHelper.Init(UIHelperDescription, () => "Меню");

            Deactivate();
        }

        protected override void OnEnterClick()
        {
            _fightMenu.Activate();

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuActivateButton))]
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