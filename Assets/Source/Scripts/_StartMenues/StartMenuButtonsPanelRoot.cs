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
        [SerializeField] private StartMenuStartButtonsPanel _startMenuStartButtonsPanel;

        public void Init(IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription)
        {
            _startMenuStartButtonsPanel.Init(SetSettingsPanel, SetRulesPanel);

            base.Init(cardVolume, musicVolume, cardCapabilityDescription, _startMenuStartButtonsPanel);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StartMenuButtonsPanelRoot))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartMenuStartButtonsPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineStartMenuStartButtonsPanel))]
        private ComponentAttachInfo DefineStartMenuStartButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuStartButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}