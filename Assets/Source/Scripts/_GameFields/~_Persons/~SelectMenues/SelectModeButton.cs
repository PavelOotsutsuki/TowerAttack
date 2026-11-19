using System.Collections.Generic;
using System.Threading;
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

        private UIHelperDescription _UIHelperDescription;

        public void Init(ISelectNumberActivator selectNumberActivator, UIHelperDescription UIHelperDescription, CancellationToken gameFieldToken)
        {
            base.Init();

            _selectNumberActivator = selectNumberActivator;
            _UIHelperDescription = UIHelperDescription;
            _UIHelper.Init(UIHelperDescription, GetHelperText, gameFieldToken);
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _selectNumberActivator.ActivateNumbers(false);
            _UIHelperDescription.SetText("Показать выбранные");
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            _selectNumberActivator.ActivateNumbers(true);
            _UIHelperDescription.SetText("Скрыть выбранные");
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