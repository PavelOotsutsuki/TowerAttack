using System;
using System.Collections;
using System.Collections.Generic;
using Menues;
using Tools.Loads;
using Tools.UI;
using Tools.UI.Extendeds;
using Tools.Utils;
using UnityEngine;
using Zenject;
using static TMPro.TMP_InputField;

namespace StartMenues.LogInButtonsPanels
{
    public class LogInButtonsPanel : CustomFocusMenuButtonsPanel
    {
        [SerializeField] private ExtendedTMP_InputField _loginIF;
        [SerializeField] private ExtendedTMP_InputField _passwordIF;
        [SerializeField] private ConfirmableFocusableButton _logInButton;
        [SerializeField] private ConfirmableFocusableButton _registraitionButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;

        private LoadRoot _loadRoot;

        //private List<ConfirmableFocusableButton> _focusableButtons;
        private Action _switchOnMainPanel;
        private string _enteredPassword = "";
        //private LogInButtonsPanelSelectHandler _selectHandler;

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
            ISelectHandler selectHandler = new LogInButtonsPanelSelectHandler(_loginIF, _passwordIF, _logInButton, _registraitionButton, _exitButton);

            List<ConfirmableFocusableButton> focusableButtons = new List<ConfirmableFocusableButton>()
            {
                _logInButton,
                _registraitionButton,
                _exitButton
            };

            base.Init(selectHandler, focusableButtons);

            _logInButton.Init(this, OnLogIn);
            _registraitionButton.Init(this, onRegistraitionButtonClick);
            _exitButton.Init(this, Utils.Quit);

            _passwordIF.inputType = InputType.Password;

            //_passwordIF.onValueChanged.AddListener(OnPasswordValueChanged);

            //_logInButton.Init(this, TestAction);
            //_registraitionButton.Init(this, TestAction);
            //_exitButton.Init(this, TestAction);
        }

        //private void OnDestroy()
        //{
        //    _passwordIF.onValueChanged.RemoveListener(OnPasswordValueChanged);
        //}

        //private void TestAction()
        //{

        //}

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
    }
}