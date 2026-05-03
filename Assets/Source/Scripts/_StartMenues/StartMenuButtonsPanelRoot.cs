using System;
using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using Menues;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace StartMenues
{
    public class StartMenuButtonsPanelRoot : MenuButtonsPanelRoot
    {
        [SerializeField] private LogInButtonsPanel _logInButtonsPanel;
        [SerializeField] private RegistrationButtonsPanel _registrationButtonsPanel;
        [SerializeField] private StartMenuStartButtonsPanel _startMenuStartButtonsPanel;

        public void Init(IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription,
            Action onPlayClick, StartMenu startMenu)
        {
            _logInButtonsPanel.Init(SetMainPanel, SetRegistrationPanel);
            _registrationButtonsPanel.Init(SetMainPanel, SetLogInPanel);
            _startMenuStartButtonsPanel.Init(SetSettingsPanel, SetRulesPanel, onPlayClick, startMenu);

            //base.Init(cardVolume, musicVolume, cardCapabilityDescription, _startMenuStartButtonsPanel);
            base.Init(cardVolume, musicVolume, cardCapabilityDescription, _logInButtonsPanel, _startMenuStartButtonsPanel);
        }

        private void SetMainPanel()
        {
            SetPanel(_startMenuStartButtonsPanel);
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
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuStartButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}