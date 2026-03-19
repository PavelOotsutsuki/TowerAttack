using System;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class StartMenuRootPrefab : RootPrefab, IAutomaticFillComponents
    {
        [SerializeField] private StartMenuRootContainer _startMenuRootContainer;

        private StartMenuRoot _currentStartMenuRoot;
        private Action _onPlayClick;

        public void Init(DiContainer diContainer, Action onPlayClick)
        {
            _onPlayClick = onPlayClick;

            base.Init(diContainer, _startMenuRootContainer);
        }

        protected override void OnActivate()
        {
            if (_currentStartMenuRoot == null)
            {
                _currentStartMenuRoot = CurrentGameObject.GetComponent<StartMenuRoot>();
                _currentStartMenuRoot.Init(_onPlayClick);
                _currentStartMenuRoot.Activate();
            }
            else
            {
                _currentStartMenuRoot.Reactivate();
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