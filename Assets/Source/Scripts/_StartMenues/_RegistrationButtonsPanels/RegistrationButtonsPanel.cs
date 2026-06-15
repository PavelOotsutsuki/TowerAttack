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
            _currentCTS?.Cancel();
            _currentCTS?.Dispose();
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_menuParentToken);
            CancellationToken token = _currentCTS.Token;

            RegistrationProcessing(token).Forget();
        }

        private async UniTask RegistrationProcessing(CancellationToken token)
        {
            LoadSession loadSession = new LoadSession();

            try
            {
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
                loadSession.Complete();
                _currentCTS?.Cancel();
                _currentCTS?.Dispose();
            }
        }

        private void OnDestroy()
        {
            _currentCTS?.Cancel();
            _currentCTS?.Dispose();
        }
    }
}