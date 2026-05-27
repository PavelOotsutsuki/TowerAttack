using System.Collections;
using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.InputSettings;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Menues
{
    public abstract class MenuButtonsPanelRoot : MonoBehaviour, IWorkable, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private MenuSettingsButtonsPanel _settingsButtonsPanel;
        [SerializeField] private MenuRulesButtonsPanel _rulesButtonsPanel;

        protected MenuButtonsPanel CurrentMenuButtonsPanel;

        private MenuButtonsPanel _mainButtonsPanel;
        private CustomFocusMenuButtonsPanel _startButtonsPanel;
        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;
        public IFocusedButtonEnterHandler CurrentMenuButtonInputHandler => CurrentMenuButtonsPanel;

        //public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, IVolume cardVolume, IVolume musicVolume,
        //    CardCapabilityDescription cardCapabilityDescription)
        //{
        //    _fadablePanel.Init();
        //    _startButtonsPanel.Init(playerLoseActions, fightMenuDeactivator, SetSettingsPanel, SetRulesPanel);
        //    _settingsButtonsPanel.Init(SetStartPanel, cardVolume, musicVolume);
        //    _rulesButtonsPanel.Init(SetStartPanel, cardCapabilityDescription);

        //    _isComplete = true;

        //    _currentFightMenuButtonsPanel = _startButtonsPanel;
        //}

        protected void Init(IVolume foregroundVolume, IVolume backgroundVolume, CardCapabilityDescription cardCapabilityDescription,
            CustomFocusMenuButtonsPanel startButtonsPanel, CustomFocusMenuButtonsPanel mainButtonsPanel)
        {
            _startButtonsPanel = startButtonsPanel;
            _mainButtonsPanel = mainButtonsPanel;

            _fadablePanel.Init();
            //_startButtonsPanel.Init(playerLoseActions, fightMenuDeactivator, SetSettingsPanel, SetRulesPanel);

            _settingsButtonsPanel.Init(SetMainPanel, foregroundVolume, backgroundVolume);
            _rulesButtonsPanel.Init(SetMainPanel, cardCapabilityDescription);

            _isComplete = true;

            CurrentMenuButtonsPanel = _startButtonsPanel;
        }

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            IsActive = true;
            _isComplete = false;

            CurrentMenuButtonsPanel = _startButtonsPanel;
            CurrentMenuButtonsPanel.Activate();

            Activating().ToUniTask();
        }

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;

            CurrentMenuButtonsPanel.Deactivate();
            CurrentMenuButtonsPanel = null;

            Deactivating().ToUniTask();
        }

        protected void SetSettingsPanel()
        {
            SetPanel(_settingsButtonsPanel);
        }

        protected void SetRulesPanel()
        {
            SetPanel(_rulesButtonsPanel);
        }

        protected void SetPanel(MenuButtonsPanel settedPanel)
        {
            if (CurrentMenuButtonsPanel == settedPanel)
                return;

            CurrentMenuButtonsPanel?.Deactivate();
            CurrentMenuButtonsPanel = settedPanel;
            CurrentMenuButtonsPanel.Activate();
        }

        private void SetMainPanel()
        {
            SetPanel(_mainButtonsPanel);
        }

        private IEnumerator Activating()
        {
            _fadablePanel.Show();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _isComplete = true;
        }

        private IEnumerator Deactivating()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(MenuButtonsPanelRoot))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel(),
                DefineMenuSettignsButtonsPanel(),
                DefineMenuRulesButtonsPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineMenuSettignsButtonsPanel))]
        private ComponentAttachInfo DefineMenuSettignsButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _settingsButtonsPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineMenuSettignsButtonsPanel))]
        private ComponentAttachInfo DefineMenuRulesButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rulesButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}