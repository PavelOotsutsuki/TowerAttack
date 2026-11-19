using System.Collections.Generic;
using System.Threading;
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
        [SerializeField] private FightMenuMainButtonsPanel _fightMenuMainButtonsPanel;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, IVolume cardVolume, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription, CancellationToken gameFieldToken)
        {
            _fightMenuMainButtonsPanel.Init(playerLoseActions, fightMenuDeactivator, SetSettingsPanel, SetRulesPanel, gameFieldToken);

            base.Init(cardVolume, musicVolume, cardCapabilityDescription, _fightMenuMainButtonsPanel, _fightMenuMainButtonsPanel, gameFieldToken);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuButtonsPanelRoot))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightMainStartButtonsPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineFightMainStartButtonsPanel))]
        private ComponentAttachInfo DefineFightMainStartButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuMainButtonsPanel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}