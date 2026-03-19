using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using GameFields.InputSettings;
using GameFields.Persons;
using Menues;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenu : Menu
    {
        [SerializeField] private FightMenuButtonsPanelRoot _fightMenuButtonsPanelRoot;

        private GameFieldInputRoot _inputRoot;

        public void Init(GameFieldInputRoot inputRoot, LoseActions playerLoseActions, IVolume cardVolume, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription)
        {
            _inputRoot = inputRoot;

            _fightMenuButtonsPanelRoot.Init(playerLoseActions, this, cardVolume, musicVolume, cardCapabilityDescription);

            base.Init(_fightMenuButtonsPanelRoot);
        }

        protected override void OnActivateInput()
        {
            _inputRoot.Pause();
        }

        protected override void OnDeactivateInput()
        {
            _inputRoot.Pause();
            _inputRoot.DeactivateFightMenu();
        }

        protected override void OnActivatingInput()
        {
            _inputRoot.ActivateFightMenu();
        }

        protected override void OnDeactivatingInput()
        {
            _inputRoot.DeactivateFightMenu();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenu))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightMenuButtonsPanel(),
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineFightMenuButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuButtonsPanelRoot, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}