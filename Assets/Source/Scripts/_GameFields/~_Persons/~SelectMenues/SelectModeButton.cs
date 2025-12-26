using System.Collections;
using System.Collections.Generic;
using GameFields.FightMenues;
using Tools.UI;
using Tools.UI.UIHelpers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    public class SelectModeButton : FadableSelectableButton
    {
        [SerializeField] private UIHelper _UIHelper;

        private ISelectNumberActivator _selectNumberActivator;

        public void Init(ISelectNumberActivator selectNumberActivator, UIHelperDescription UIHelperDescription)
        {
            base.Init();

            _selectNumberActivator = selectNumberActivator;
            _UIHelper.Init(UIHelperDescription, GetHelperText);
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _selectNumberActivator.ActivateNumbers(false);
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            _selectNumberActivator.ActivateNumbers(true);
        }

        private string GetHelperText()
        {
            if (IsClicked)
                return "Показать выбранные";

            return "Скрыть выбранные";
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectModeButton))]
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