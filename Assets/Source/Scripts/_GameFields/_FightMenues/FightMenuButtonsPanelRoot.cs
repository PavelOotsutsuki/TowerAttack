using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Commons;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenuButtonsPanelRoot : MonoBehaviour, IWorkable, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private FightMenuStartButtonsPanel _startButtonsPanel;
        [SerializeField] private FightMenuSettingsButtonsPanel _settingsButtonsPanel;
        [SerializeField] private FightMenuRulesButtonsPanel _rulesButtonsPanel;

        private FightMenuButtonsPanel _currentFightMenuButtonsPanel;
        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;
        public IFocusedButtonEnterHandler CurrentFightMenuButtonInputHandler => _currentFightMenuButtonsPanel;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, IVolume cardVolume, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription)
        {
            _fadablePanel.Init();
            _startButtonsPanel.Init(playerLoseActions, fightMenuDeactivator, SetSettingsPanel, SetRulesPanel);
            _settingsButtonsPanel.Init(SetStartPanel, cardVolume, musicVolume);
            _rulesButtonsPanel.Init(SetStartPanel, cardCapabilityDescription);

            _isComplete = true;

            _currentFightMenuButtonsPanel = _startButtonsPanel;
        }

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            IsActive = true;
            _isComplete = false;

            _currentFightMenuButtonsPanel = _startButtonsPanel;
            _currentFightMenuButtonsPanel.Activate();

            Activating().ToUniTask();
        }

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;

            _currentFightMenuButtonsPanel.Deactivate();
            _currentFightMenuButtonsPanel = null;

            Deactivating().ToUniTask();
        }

        private void SetStartPanel()
        {
            SetPanel(_startButtonsPanel);
        }

        private void SetSettingsPanel()
        {
            SetPanel(_settingsButtonsPanel);
        }

        private void SetRulesPanel()
        {
            SetPanel(_rulesButtonsPanel);
        }

        private void SetPanel(FightMenuButtonsPanel settedPanel)
        {
            if (_currentFightMenuButtonsPanel == settedPanel)
                return;

            _currentFightMenuButtonsPanel?.Deactivate();
            _currentFightMenuButtonsPanel = settedPanel;
            _currentFightMenuButtonsPanel.Activate();
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
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuButtonsPanelRoot))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel(),
                DefineFightMenuStartButtonsPanel(),
                DefineFightMenuSettignsButtonsPanel(),
                DefineFightMenuRulesButtonsPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFightMenuStartButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuStartButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startButtonsPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuSettignsButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuSettignsButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _settingsButtonsPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuSettignsButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuRulesButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rulesButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}