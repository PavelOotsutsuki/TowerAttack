using System;
using System.Collections;
using System.Collections.Generic;
using Menues;
using TMPro;
using Tools.Loads;
using Tools.UI;
using Tools.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace StartMenues
{
    public class RegistrationButtonsPanel : CustomFocusMenuButtonsPanel
    {
        //[SerializeField] private TMP_InputField _registraitionButton;
        //[SerializeField] private TMP_InputField _registraitionButton;
        [SerializeField] private ConfirmableFocusableButton _registraitionButton;
        [SerializeField] private ConfirmableFocusableButton _backButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;
        
        private LoadRoot _loadRoot;

        private List<ConfirmableFocusableButton> _startMenuButtons;
        private Action _switchOnMainPanel;

        private ConfirmableFocusableButton _currentFocusedButton;

        [Inject]
        public void Construct(LoadRoot loadRoot)
        {
            _loadRoot = loadRoot;
        }

        public void Init(Action switchOnMainPanel, Action onBackButtonClick)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _switchOnMainPanel = switchOnMainPanel;

            _startMenuButtons = new List<ConfirmableFocusableButton>()
            {
                _registraitionButton,
                _backButton,
                _exitButton
            };

            _backButton.Init(this, onBackButtonClick);
            _registraitionButton.Init(this, OnRegistration);
            _exitButton.Init(this, Utils.Quit);
        }

        private void OnRegistration()
        {
            StartCoroutine(RegistrationProcessing());
        }

        private IEnumerator RegistrationProcessing()
        {
            _loadRoot.Activate();

            yield return new WaitForSeconds(3f);

            _loadRoot.Deactivate();

            _switchOnMainPanel.Invoke();
        }

        public override void OnEnterPress()
        {
            _currentFocusedButton?.OnPointerClick(null);
        }

        public override void OnDownArrow()
        {
            if (_currentFocusedButton == null)
                return;

            int index = _startMenuButtons.IndexOf(_currentFocusedButton);

            do
            {
                index++;

                if (index == _startMenuButtons.Count)
                    index = 0;
            }
            while (_startMenuButtons[index].IsDisable);

            _startMenuButtons[index].OnPointerEnter(null);
        }

        public override void OnUpArrow()
        {
            if (_currentFocusedButton == null)
                return;

            int index = _startMenuButtons.IndexOf(_currentFocusedButton);

            do
            {
                index--;

                if (index < 0)
                    index = _startMenuButtons.Count - 1;
            }
            while (_startMenuButtons[index].IsDisable);

            _startMenuButtons[index].OnPointerEnter(null);
        }

        public override void SetFocused(ConfirmableFocusableButton focusedButton)
        {
            if (_currentFocusedButton == focusedButton)
                return;

            if (_startMenuButtons.Contains(focusedButton) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе кнопку???");

            UnfocuseButton();

            foreach (StartMenuButton startMenuButton in _startMenuButtons)
            {
                if (startMenuButton == focusedButton)
                {
                    _currentFocusedButton = startMenuButton;
                    _currentFocusedButton.PointerDisableSettingsRoot.OnPointerExit.Disable();
                    return;
                }
            }
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            base.Activate();

            foreach (StartMenuButton startMenuButton in _startMenuButtons)
            {
                startMenuButton.Activate();
            }

            EventSystem.current.SetSelectedGameObject(null);
            _startMenuButtons[0].OnPointerEnter(null);
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            base.Deactivate();

            foreach (StartMenuButton startMenuButton in _startMenuButtons)
            {
                startMenuButton.Deactivate();
            }
        }

        private void UnfocuseButton()
        {
            if (_currentFocusedButton != null)
            {
                _currentFocusedButton.PointerDisableSettingsRoot.OnPointerExit.Enable();
                _currentFocusedButton.OnPointerExit(null);
                _currentFocusedButton = null;
            }
        }

        public override void OnLeftArrow()
        {
            throw new NotImplementedException();
        }

        public override void OnRightArrow()
        {
            throw new NotImplementedException();
        }
    }
}