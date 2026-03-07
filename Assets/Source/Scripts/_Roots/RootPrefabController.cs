using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class RootPrefabController : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private StartMenuRootPrefab _startMenuRootPrefab;
        [SerializeField] private GameFieldRootPrefab _gameFieldRootPrefab;

        private RootPrefab _currentRootPrefab;

        public void Init(DiContainer diContainer)
        {
            _startMenuRootPrefab.Init(diContainer, () => SwitchPrefab(_gameFieldRootPrefab));
            _gameFieldRootPrefab.Init(diContainer, () => SwitchPrefab(_startMenuRootPrefab));
        }

        public void SwitchPrefab(RootPrefabType rootPrefabType)
        {
            RootPrefab activatingRootPrefab = rootPrefabType switch
            {
                RootPrefabType.StartMenu => _startMenuRootPrefab,
                RootPrefabType.GameField => _gameFieldRootPrefab,
                _ => throw new Exception($"Неизвестный {nameof(RootPrefabType)}: {rootPrefabType}")
            };

            SwitchPrefab(activatingRootPrefab);
        }

        private void SwitchPrefab(RootPrefab activatingRootPrefab)
        {
            _currentRootPrefab?.Deactivate();
            _currentRootPrefab = activatingRootPrefab;
            _currentRootPrefab.Activate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(RootPrefabController))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartMenuRootPrefab(),
                DefineGameFieldRootPrefab()
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartMenuRootPrefab))]
        private ComponentAttachInfo DefineStartMenuRootPrefab()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startMenuRootPrefab, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineGameFieldRootPrefab))]
        private ComponentAttachInfo DefineGameFieldRootPrefab()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _gameFieldRootPrefab, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}