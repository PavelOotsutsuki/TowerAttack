using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Menues;
using Servers;
using Servers.DTO;
using TMPro;
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
        [SerializeField] private Label _errorLabel;

        private LoadRoot _loadRoot;
        private DBRoot _dBRoot;

        private CancellationToken _menuParentToken;
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

        public void Init(Action switchOnMainPanel, Action onRegistraitionButtonClick, CancellationToken menuParentToken)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _switchOnMainPanel = switchOnMainPanel;
            _menuParentToken = menuParentToken;
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
            _errorLabel.gameObject.SetActive(false);

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
            _errorLabel.gameObject.SetActive(false);
            //_tokenSource?.Dispose();
            Utils.DestroyCTS(ref _tokenSource);
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_menuParentToken);
            CancellationToken token = _tokenSource.Token;

            LogInProcessing(token).Forget();
        }

        private async UniTask LogInProcessing(CancellationToken token)
        {
            LoadSession loadSession = new LoadSession();

            try
            {
                CheckInputData();

                _loadRoot.AddSession(loadSession);

                await _dBRoot.LogInUser(_loginIF.text.Trim(), _passwordIF.text, token);
                //Debug.Log(user);
                //_userData.SetUserData(user);
                await UniTask.Delay(1000, cancellationToken: token);
                //yield return new WaitForSeconds(3f);

                loadSession.Complete();

                _switchOnMainPanel.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log($"Ошибка {nameof(LogInButtonsPanel)}-->{nameof(LogInProcessing)}: {ex.Message}");
                _logInButton.Deactivate();
                _logInButton.Activate();
                _errorLabel.gameObject.SetActive(true);
                _errorLabel.SetText(ex.Message);
                loadSession.Complete();
                _tokenSource.Cancel();
            }
        }

        private void CheckInputData()
        {
            //StringBuilder errMsg = new StringBuilder();

            CheckLogin();
            CheckPassword();

            //if (errMsg.ToString() != "")
            //    throw new Exception(errMsg.ToString());
        }

        private void CheckLogin()
        {
            string login = _loginIF.text;

            if (login == "")
                throw new Exception("Логин не может быть пустым!");

            if (Regex.IsMatch(login, @"^[a-zA-Z][a-zA-Z0-9_]{3,20}$") == false)
                throw new Exception("Логин должен начинаться с буквы, иметь в себе только латинские буквы, цифры и _, не менее 3 символов и не более 20");
        }

        private void CheckPassword()
        {
            string password = _passwordIF.text;

            if (password.Length < 3)
                throw new Exception("Пароль не может быть меньше 3 символов!");



            //if (login == "")
            //    errMsg.Append("Логин не может быть пустым!");

            //if (Regex.IsMatch(login, @"^[a-zA-Z][a-zA-Z0-9_]{3,20}$") == false)
            //    errMsg.Append("Логин должен начинаться с буквы, иметь в себе только латинские буквы, цифры и _, не менее 3 символов и не более 20");
        }

        private void OnDestroy()
        {
            Utils.DestroyCTS(ref _tokenSource);
        }
    }
}