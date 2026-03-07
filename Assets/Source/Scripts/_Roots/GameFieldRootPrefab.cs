using System;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class GameFieldRootPrefab : RootPrefab, IAutomaticFillComponents
    {
        [SerializeField] private GameFieldRootContainer _container;

        private GameFieldRoot _currentGameFieldRoot;
        private Action _onDestroyPrefab;

        public void Init(DiContainer diContainer, Action onDestroyPrefab)
        {
            _onDestroyPrefab = onDestroyPrefab;

            base.Init(diContainer, _container);
        }

        protected override void OnActivate()
        {
            _currentGameFieldRoot = CurrentGameObject.GetComponent<GameFieldRoot>();
            _currentGameFieldRoot.Init(_onDestroyPrefab);
        }

        protected override void OnDeactivate()
        {
            Destroy(CurrentGameObject);
            CurrentGameObject = null;
            _currentGameFieldRoot = null;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(GameFieldRootPrefab))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineGameFieldRootContainer()
            };

            return list;
        }

        [ContextMenu(nameof(DefineGameFieldRootContainer))]
        private ComponentAttachInfo DefineGameFieldRootContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _container, ComponentLocationTypes.InThisElseChildren);
        }
        #endregion
    }
}