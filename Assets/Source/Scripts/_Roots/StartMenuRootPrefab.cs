using System;
using System.Collections.Generic;
using System.Threading;
using StartMenues;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class StartMenuRootPrefab : RootPrefab, IAutomaticFillComponents
    {
        [SerializeField] private StartMenuRootContainer _startMenuRootContainer;

        private StartMenuRoot _currentStartMenuRoot;
        private Action<int> _onPlayClick;

        public void Init(DiContainer diContainer, Action<int> onPlayClick, CancellationToken gameRootToken)
        {
            _onPlayClick = onPlayClick;

            base.Init(diContainer, _startMenuRootContainer, gameRootToken);
        }

        protected override void OnActivate()
        {
            if (_currentStartMenuRoot == null)
            {
                _currentStartMenuRoot = CurrentGameObject.GetComponent<StartMenuRoot>();
                _currentStartMenuRoot.Init(_onPlayClick);
                _currentStartMenuRoot.Activate();
                IsComplete = true;
            }
            else
            {
                StartMenuButtonsPanelRootReactivateData reactivateDataInvoker = new StartMenuButtonsPanelRootReactivateData(() => IsComplete = true);
                _currentStartMenuRoot.Reactivate(reactivateDataInvoker);
            }
        }

        protected override void OnDeactivate()
        {
            CurrentGameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StartMenuRootPrefab))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartMenuRootContainer()
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartMenuRootContainer))]
        private ComponentAttachInfo DefineStartMenuRootContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuRootContainer, ComponentLocationTypes.InThisElseChildren);
        }
        #endregion
    }
}