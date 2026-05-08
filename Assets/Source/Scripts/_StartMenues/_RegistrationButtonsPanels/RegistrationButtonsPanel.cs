using System;
using System.Collections;
using System.Collections.Generic;
using Menues;
using Tools.Loads;
using Tools.UI;
using Tools.UI.Extendeds;
using Tools.Utils;
using UnityEngine;
//using UnityEngine.EventSystems;
using Zenject;

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

        //private List<ConfirmableFocusableButton> _focusableButtons;
        private Action _switchOnMainPanel;
        //private RegistrationButtonsPanelSelectHandler _selectHandler;

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
    }
}