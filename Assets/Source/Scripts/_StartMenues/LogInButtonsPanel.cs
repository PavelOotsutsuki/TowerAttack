using System;
using System.Collections;
using System.Collections.Generic;
using Menues;
using StartMenues.LogInButtonsPanels;
using TMPro;
using Tools.Loads;
using Tools.UI;
using Tools.UI.Extendeds;
using Tools.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace StartMenues
{
    public class LogInButtonsPanel : CustomFocusMenuButtonsPanel
    {
        [SerializeField] private ExtendedTMP_InputField _loginIF;
        [SerializeField] private ExtendedTMP_InputField _passwordIF;
        [SerializeField] private ConfirmableFocusableButton _logInButton;
        [SerializeField] private ConfirmableFocusableButton _registraitionButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;

        private LoadRoot _loadRoot;

        private List<ConfirmableFocusableButton> _focusableButtons;
        private Action _switchOnMainPanel;
        private LogInButtonsPanelSelectHandler _logInButtonsPanelSelectHandler;

        [Inject]
        public void Construct(LoadRoot loadRoot)
        {
            _loadRoot = loadRoot;
        }

        public void Init(Action switchOnMainPanel, Action onRegistraitionButtonClick)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _switchOnMainPanel = switchOnMainPanel;
            _logInButtonsPanelSelectHandler = new LogInButtonsPanelSelectHandler(_loginIF, _passwordIF, _logInButton, _registraitionButton, _exitButton);

            _focusableButtons = new List<ConfirmableFocusableButton>()
            {
                _logInButton,
                _registraitionButton,
                _exitButton
            };

            _logInButton.Init(this, OnLogIn);
            _registraitionButton.Init(this, onRegistraitionButtonClick);
            _exitButton.Init(this, Utils.Quit);
        }

        private void OnLogIn()
        {
            StartCoroutine(LogInProcessing());
        }

        private IEnumerator LogInProcessing()
        {
            _loadRoot.Activate();

            yield return new WaitForSeconds(3f);

            _loadRoot.Deactivate();

            _switchOnMainPanel.Invoke();
        }

        public override void OnEnterPress()
        {
            _logInButtonsPanelSelectHandler.OnEnterPress();
        }

        public override void OnDownArrow()
        {
            _logInButtonsPanelSelectHandler.OnDownArrow();
        }

        public override void OnUpArrow()
        {
            _logInButtonsPanelSelectHandler.OnUpArrow();
        }

        public override void OnLeftArrow()
        {
            _logInButtonsPanelSelectHandler.OnLeftArrow();
        }

        public override void OnRightArrow()
        {
            _logInButtonsPanelSelectHandler.OnRightArrow();
        }

        public override void SetFocused(ConfirmableFocusableButton focused)
        {
            _logInButtonsPanelSelectHandler.SetFocused(focused);
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
            //_loginIF.DeactivateInputField();
            //_passwordIF.DeactivateInputField();
            _logInButtonsPanelSelectHandler.Activate();
            //_startMenuButtons[0].OnPointerEnter(null); 
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
    }
}