using System;
using System.Collections.Generic;
using Tools.UI;

namespace Menues
{
    public class MainButtonsPanelSelectHandler : ISelectHandler
    {
        private readonly List<ConfirmableFocusableButton> _allFocusable;

        private ConfirmableFocusableButton _currentFocused;

        public MainButtonsPanelSelectHandler(List<ConfirmableFocusableButton> allFocusable)
        {
            _allFocusable = allFocusable;

            _currentFocused = null;
        }

        public void Activate()
        {
            _allFocusable[0].OnPointerEnter(null);
        }

        public void OnEnterPress()
        {
            _currentFocused?.OnPointerClick(null);
        }

        public void OnDownArrow()
        {
            if (_currentFocused == null)
                return;

            int index = _allFocusable.IndexOf(_currentFocused);

            do
            {
                index++;

                if (index == _allFocusable.Count)
                    index = 0;
            }
            while (_allFocusable[index].IsDisable);

            _allFocusable[index].OnPointerEnter(null);
        }

        public void OnUpArrow()
        {
            if (_currentFocused == null)
                return;

            int index = _allFocusable.IndexOf(_currentFocused);

            do
            {
                index--;

                if (index < 0)
                    index = _allFocusable.Count - 1;
            }
            while (_allFocusable[index].IsDisable);

            _allFocusable[index].OnPointerEnter(null);
        }

        public void OnLeftArrow()
        { }

        public void OnRightArrow()
        { }

        public void SetFocused(ConfirmableFocusableButton focusedButton)
        {
            if (_currentFocused == focusedButton)
                return;

            if (_allFocusable.Contains(focusedButton) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе кнопку???");

            UnfocuseButton();

            foreach (ConfirmableFocusableButton startMenuButton in _allFocusable)
            {
                if (startMenuButton == focusedButton)
                {
                    _currentFocused = startMenuButton;
                    _currentFocused.PointerDisableSettingsRoot.OnPointerExit.Disable();
                    return;
                }
            }
        }

        private void UnfocuseButton()
        {
            if (_currentFocused != null)
            {
                _currentFocused.PointerDisableSettingsRoot.OnPointerExit.Enable();
                _currentFocused.OnPointerExit(null);
                _currentFocused = null;
            }
        }
    }
}