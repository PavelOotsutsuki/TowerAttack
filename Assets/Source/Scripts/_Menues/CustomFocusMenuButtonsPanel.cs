using System.Collections.Generic;
using Tools.UI;
using UnityEngine.EventSystems;

namespace Menues
{
    public abstract class CustomFocusMenuButtonsPanel : MenuButtonsPanel, IFocusCustomButtonWatcher//, IAutomaticFillComponents
    {
        private ISelectHandler _selectHandler;
        private IEnumerable<ConfirmableFocusableButton> _focusableButtons;

        protected void Init(ISelectHandler selectHandler, IEnumerable<ConfirmableFocusableButton> focusableButtons)
        {
            _selectHandler = selectHandler;
            _focusableButtons = focusableButtons;
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            base.Activate();

            foreach (ConfirmableFocusableButton focusableButton in _focusableButtons)
            {
                focusableButton.Activate();
            }

            EventSystem.current.SetSelectedGameObject(null);
            _selectHandler.Activate();
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            base.Deactivate();

            foreach (ConfirmableFocusableButton focusableButton in _focusableButtons)
            {
                focusableButton.Deactivate();
            }
        }

        public override void OnEnterPress()
        {
            _selectHandler.OnEnterPress();
        }

        public override void OnDownArrow()
        {
            _selectHandler.OnDownArrow();
        }

        public override void OnUpArrow()
        {
            _selectHandler.OnUpArrow();
        }

        public override void OnLeftArrow()
        {
            _selectHandler.OnLeftArrow();
        }

        public override void OnRightArrow()
        {
            _selectHandler.OnRightArrow();
        }

        public void SetFocused(ConfirmableFocusableButton focusedButton)
        {
            _selectHandler.SetFocused(focusedButton);
        }
    }
}