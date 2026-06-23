using System;
using System.Collections.Generic;
using System.Threading;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using Menues;
using Servers.DTO;
using StartMenues.LogInButtonsPanels;
using StartMenues.RegistrationButtonsPanels;
using Tools;
using Tools.Loads;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace StartMenues
{
    public class StartMenuButtonsPanelRoot : MenuButtonsPanelRoot, IReactivatable<StartMenuButtonsPanelRootReactivateData>
    {
        [SerializeField] private LogInButtonsPanel _logInButtonsPanel;
        [SerializeField] private RegistrationButtonsPanel _registrationButtonsPanel;
        [SerializeField] private StartMenuMainButtonsPanel _startMenuMainButtonsPanel;
        [Inject] private LoadRoot _loadRoot;

        public void Init(IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription,
            Action<int> onPlayClick, StartMenu startMenu, CancellationToken startMenuToken)
        {
            _logInButtonsPanel.Init(SetMainPanel, SetRegistrationPanel);
            _registrationButtonsPanel.Init(SetMainPanel, SetLogInPanel, startMenuToken);
            _startMenuMainButtonsPanel.Init(SetSettingsPanel, SetRulesPanel, onPlayClick, startMenu, startMenuToken);

            //base.Init(cardVolume, musicVolume, cardCapabilityDescription, _startMenuStartButtonsPanel);
            base.Init(cardVolume, musicVolume, cardCapabilityDescription, _logInButtonsPanel, _startMenuMainButtonsPanel, startMenuToken);
        }

        public void Reactivate(StartMenuButtonsPanelRootReactivateData reactivateDataInvoker)
        {
            CurrentMenuButtonsPanel?.Deactivate();
            CurrentMenuButtonsPanel = _startMenuMainButtonsPanel;

            ReactivatingStartMenuMainButtonsPanel(reactivateDataInvoker, MenuParentToken).Forget();


            //_startMenuMainButtonsPanel.Preactivate();
            ////MainButtonsPanel.Re
            //CurrentMenuButtonsPanel.Activate();
        }

        private async UniTask ReactivatingStartMenuMainButtonsPanel(IActivatable reactivateDataInvoker, CancellationToken token)
        {
            await _startMenuMainButtonsPanel.Preactivate(token);
            //MainButtonsPanel.Re
            CurrentMenuButtonsPanel.Activate();
            reactivateDataInvoker.Activate();
        }

        private void SetMainPanel()
        {
            SettingMainPanel().Forget();
        }

        private async UniTask SettingMainPanel()
        {
            CancellationTokenSource loadActualPersonDataFromDBCTS = CancellationTokenSource.CreateLinkedTokenSource(MenuParentToken,
                new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);

            LoadSession loadSession = new LoadSession();
            _loadRoot.AddSession(loadSession);

            try
            {
                await _startMenuMainButtonsPanel.Preactivate(loadActualPersonDataFromDBCTS.Token);
            }
            finally
            {
                Utils.DestroyCTS(ref loadActualPersonDataFromDBCTS);
            }

            loadSession.Complete();

            SetPanel(_startMenuMainButtonsPanel);
        }

        private void SetLogInPanel()
        {
            SetPanel(_logInButtonsPanel);
        }

        private void SetRegistrationPanel()
        {
            SetPanel(_registrationButtonsPanel);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StartMenuButtonsPanelRoot))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLogInButtonsPanel(),
                DefineRegistrationButtonsPanel(),
                DefineStartMenuStartButtonsPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineLogInButtonsPanel))]
        private ComponentAttachInfo DefineLogInButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _logInButtonsPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRegistrationButtonsPanel))]
        private ComponentAttachInfo DefineRegistrationButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _registrationButtonsPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineStartMenuStartButtonsPanel))]
        private ComponentAttachInfo DefineStartMenuStartButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuMainButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}