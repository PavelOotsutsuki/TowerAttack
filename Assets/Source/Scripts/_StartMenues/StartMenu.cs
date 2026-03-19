using System;
using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using Menues;
using StartMenues.InputSettings;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace StartMenues
{
    public class StartMenu : Menu
    {
        [SerializeField] private StartMenuButtonsPanelRoot _startMenuButtonsPanelRoot;

        private StartMenuInputRoot _inputRoot;

        public void Init(/*InputRoot inputRoot, */IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription,
            Action onPlayClick)
        {
            //_inputRoot = inputRoot;

            _inputRoot = new StartMenuInputRoot(this);

            _startMenuButtonsPanelRoot.Init(cardVolume, musicVolume, cardCapabilityDescription, onPlayClick, this);

            base.Init(_startMenuButtonsPanelRoot);
            _inputRoot.Activate();
        }

        public override void Hide()
        {
            _inputRoot.Deactivate();
        }

        protected override void DeactivateChilds()
        { }

        protected override void OnActivateInput()
        {
            _inputRoot.Deactivate();
        }

        protected override void OnDeactivateInput()
        {
            //_inputRoot.Pause();
            _inputRoot.Deactivate();
            //_inputRoot.Unpause();
            //_inputRoot.DeactivateFightMenu();
        }

        protected override void OnActivatingInput()
        {
            _inputRoot.Activate();
            //_inputRoot.Unpause();
            //_inputRoot.ActivateFightMenu();
        }

        protected override void OnDeactivatingInput()
        {
            _inputRoot.Deactivate();
            //_inputRoot.Unpause();
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