using System.Collections;
using System.Collections.Generic;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    public class SelectModeButton : FadableSelectableButton
    {
        private ISelectNumberActivator _selectNumberActivator;

        public void Init(ISelectNumberActivator selectNumberActivator)
        {
            base.Init();

            _selectNumberActivator = selectNumberActivator;
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
    }
}