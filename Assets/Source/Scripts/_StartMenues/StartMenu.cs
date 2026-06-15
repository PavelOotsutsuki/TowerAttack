using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cards.Views.BigCardViews.Capabilities;
using Menues;
using StartMenues.InputSettings;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace StartMenues
{
    public class StartMenu : Menu, IHidable, IReactivatable<StartMenuButtonsPanelRootReactivateData>
    {
        [SerializeField] private StartMenuButtonsPanelRoot _startMenuButtonsPanelRoot;

        private StartMenuInputRoot _inputRoot;
        //private CancellationTokenSource _startMenuCTS;

        public void Init(StartMenuInputRoot inputRoot, IVolume cardVolume, IVolume musicVolume, CardCapabilityDescription cardCapabilityDescription,
            Action<int> onPlayClick, CancellationToken startMenuRootToken)
        {
            //_inputRoot = inputRoot;

            _inputRoot = inputRoot;

            _startMenuButtonsPanelRoot.Init(cardVolume, musicVolume, cardCapabilityDescription, onPlayClick, this, startMenuRootToken);

            base.Init(_startMenuButtonsPanelRoot, startMenuRootToken);
            _inputRoot.Activate();
        }

        public void Reactivate(StartMenuButtonsPanelRootReactivateData reactivateDataInvoker)
        {
            IsComplete = false;
            _startMenuButtonsPanelRoot.Reactivate(reactivateDataInvoker);

            OnActivatingInput();
            IsComplete = true;
        }

        public void Hide()
        {
            _inputRoot.Deactivate();
        }

        protected override void OnActivateInput()
        {
            _inputRoot.Pause();
        }

        protected override void OnDeactivateInput()
        {
            //_inputRoot.Pause();
            _inputRoot.Pause();
            //_inputRoot.Unpause();
            //_inputRoot.DeactivateFightMenu();
        }

        protected override void OnActivatingInput()
        {
            _inputRoot.Unpause();
            //_inputRoot.ActivateFightMenu();
        }

        protected override void OnDeactivatingInput()
        {
            _inputRoot.Unpause();
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