using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using Menues;
using Servers;
using Tools.Loads;
using Tools.UI;
using Tools.UI.Extendeds;
using Tools.Utils;
using UnityEngine;
//using UnityEngine.EventSystems;
using Zenject;
using static TMPro.TMP_InputField;
using System.Text;
using System.Text.RegularExpressions;

namespace StartMenues.RegistrationButtonsPanels
{
    public class RegistrationButtonsPanel : CustomFocusMenuButtonsPanel
    {
        [SerializeField] private ExtendedTMP_InputField _loginIF;
        [SerializeField] private ExtendedTMP_InputField _passwordIF;
        [SerializeField] private ExtendedTMP_InputField _passwordAgainIF;
        [SerializeField] private ConfirmableFocusableButton _registraitionButton;
        [SerializeField] private ConfirmableFocusableButton _backButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;
        [SerializeField] private Label _errorLabel;

        private LoadRoot _loadRoot;
        private DBRoot _dBRoot;

        private CancellationToken _menuParentToken;
        private CancellationTokenSource _currentCTS;

        //private List<ConfirmableFocusableButton> _focusableButtons;
        private Action _switchOnMainPanel;
        //private RegistrationButtonsPanelSelectHandler _selectHandler;

        [Inject]
        public void Construct(LoadRoot loadRoot, DBRoot dBRoot)
        {
            _loadRoot = loadRoot;
            _dBRoot = dBRoot;
        }

        public void Init(Action switchOnMainPanel, Action onBackButtonClick, CancellationToken menuParentToken)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _switchOnMainPanel = switchOnMainPanel;
            _menuParentToken = menuParentToken;

            ISelectHandler selectHandler = new RegistrationButtonsPanelSelectHandler(_loginIF, _passwordIF, _passwordAgainIF, _registraitionButton,
                _backButton, _exitButton);

            List<ConfirmableFocusableButton> focusableButtons = new List<ConfirmableFocusableButton>()
            {
                _registraitionButton,
                _backButton,
                _exitButton
            };

            base.Init(selectHandler, focusableButtons);

            _backButton.Init(this, onBackButtonClick);
            _registraitionButton.Init(this, OnRegistration);
            _exitButton.Init(this, Utils.Quit);

            _passwordIF.inputType = InputType.Password;
            _passwordAgainIF.inputType = InputType.Password;

            _errorLabel.gameObject.SetActive(false);
        }

        //private void OnRegistration()
        //{
        //    StartCoroutine(RegistrationProcessing());
        //}

        //private IEnumerator RegistrationProcessing()
        //{
        //    _loadRoot.Activate();

        //    yield return new WaitForSeconds(3f);

        //    _loadRoot.Deactivate();

        //    _switchOnMainPanel.Invoke();
        //}

        private void OnRegistration()
        {
            _errorLabel.gameObject.SetActive(false);
            Utils.DestroyCTS(ref _currentCTS);

            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_menuParentToken);
            CancellationToken token = _currentCTS.Token;

            RegistrationProcessing(token).Forget();
        }

        private async UniTask RegistrationProcessing(CancellationToken token)
        {
            LoadSession loadSession = new LoadSession();

            try
            {
                CheckInputData();

                _loadRoot.AddSession(loadSession);

                await _dBRoot.CreateUser(_loginIF.text.Trim(), _passwordIF.text, token);
                //Debug.Log(user);
                //_userData.SetUserData(user);
                await UniTask.Delay(1000, cancellationToken: token);
                //yield return new WaitForSeconds(3f);

                loadSession.Complete();

                _switchOnMainPanel.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log($"Ошибка {nameof(RegistrationButtonsPanel)}-->{nameof(RegistrationProcessing)}: {ex.Message}");
                _registraitionButton.Deactivate();
                _registraitionButton.Activate();
                _errorLabel.gameObject.SetActive(true);
                _errorLabel.SetText(ex.Message);
                loadSession.Complete();
                Utils.DestroyCTS(ref _currentCTS);
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
            if (_passwordIF.text != _passwordAgainIF.text)
                throw new Exception("Пароли не совпадают!");

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
            Utils.DestroyCTS(ref _currentCTS);
        }
    }
}