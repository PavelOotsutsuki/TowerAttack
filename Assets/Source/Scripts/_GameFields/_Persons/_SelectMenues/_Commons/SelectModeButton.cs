using System.Collections;
using System.Collections.Generic;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectModeButton : SelectableButton
    {
        private SelectNumberPanelPlayer _selectNumberPanelPlayer;

        public void Init(SelectNumberPanelPlayer selectNumberPanelPlayer)
        {
            base.Init();

            _selectNumberPanelPlayer = selectNumberPanelPlayer;
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _selectNumberPanelPlayer.FullActivate();
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            _selectNumberPanelPlayer.DefaultActivate();
        }
    }
}