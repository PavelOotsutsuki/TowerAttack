using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using Menues;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace StartMenues
{
    public class StartMenu : Menu
    {
        [SerializeField] private StartMenuButtonsPanelRoot _startMenuButtonsPanelRoot;

        //private InputRoot _inputRoot;

        public void Init(/*InputRoot inputRoot, */IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription,
            IActivatable gameRootActivatable)
        {
            //_inputRoot = inputRoot;

            _startMenuButtonsPanelRoot.Init(cardVolume, musicVolume, cardCapabilityDescription, gameRootActivatable, this);

            base.Init(_startMenuButtonsPanelRoot);
        }

        protected override void OnActivateInput()
        {
            //_inputRoot.Pause();
        }

        protected override void OnDeactivateInput()
        {
            //_inputRoot.Pause();
            //_inputRoot.DeactivateFightMenu();
        }

        protected override void OnActivatingInput()
        {
            //_inputRoot.ActivateFightMenu();
        }

        protected override void OnDeactivatingInput()
        {
            //_inputRoot.DeactivateFightMenu();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StartMenu))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartMenuButtonsPanel(),
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineStartMenuButtonsPanel))]
        private ComponentAttachInfo DefineStartMenuButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuButtonsPanelRoot, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}