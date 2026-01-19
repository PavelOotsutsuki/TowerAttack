using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using GameFields.Persons;
using Menues;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenuButtonsPanelRoot : MenuButtonsPanelRoot
    {
        [SerializeField] private FightMenuStartButtonsPanel _fightMenuStartButtonsPanel;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, IVolume cardVolume, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription)
        {
            _fightMenuStartButtonsPanel.Init(playerLoseActions, fightMenuDeactivator, SetSettingsPanel, SetRulesPanel);

            base.Init(cardVolume, musicVolume, cardCapabilityDescription, _fightMenuStartButtonsPanel);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuButtonsPanelRoot))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightMenuStartButtonsPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineFightMenuStartButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuStartButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuStartButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}