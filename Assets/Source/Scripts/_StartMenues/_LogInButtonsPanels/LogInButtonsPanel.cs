using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Menues;
using Servers;
using Servers.DTO;
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
        private DBRoot _dBRoot;
        private CancellationTokenSource _tokenSource;

        //private List<ConfirmableFocusableButton> _focusableButtons;
        private Action _switchOnMainPanel;
        private string _enteredPassword = "";
        //private LogInButtonsPanelSelectHandler _selectHandler;

        [Inject]
        public void Construct(LoadRoot loadRoot, DBRoot dBRoot)
        {
            _loadRoot = loadRoot;
            _dBRoot = dBRoot;
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
            _tokenSource?.Dispose();
            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;

            LogInProcessing(token).Forget();
        }

        private async UniTask LogInProcessing(CancellationToken token)
        {
            try
            {
                _loadRoot.Activate();

                await _dBRoot.LogInUser(_loginIF.text.Trim(), _passwordIF.text, token);
                //Debug.Log(user);
                //_userData.SetUserData(user);
                await UniTask.Delay(1000, cancellationToken: token);
                //yield return new WaitForSeconds(3f);

                _loadRoot.Deactivate();

                _switchOnMainPanel.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log($"Ошибка {nameof(LogInButtonsPanel)}-->{nameof(LogInProcessing)}: {ex.Message}");
                _logInButton.Deactivate();
                _logInButton.Activate();
                _loadRoot.Deactivate();
                _tokenSource.Cancel();
            }
        }

        private void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
    }
}