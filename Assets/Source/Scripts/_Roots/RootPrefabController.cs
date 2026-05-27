using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Servers;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class RootPrefabController : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private SwitchRootPrefabPanel _switchRootPrefabPanel;
        [SerializeField] private StartMenuRootPrefab _startMenuRootPrefab;
        [SerializeField] private GameFieldRootPrefab _gameFieldRootPrefab;
        [Inject] private DBRoot _dBRoot;

        private RootPrefab _currentRootPrefab;

        public void Init(DiContainer diContainer)
        {
            _switchRootPrefabPanel.Init();
            _startMenuRootPrefab.Init(diContainer, StartFight);
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

        private void StartFight(int id_mode)
        {
            StartingFight(id_mode, this.destroyCancellationToken).Forget();
        }

        private async UniTask StartingFight(int id_mode, CancellationToken token)
        {
            await _dBRoot.StartFightWithBot(id_mode, token);

            SwitchPrefab(_gameFieldRootPrefab);
        }

        private void SwitchPrefab(RootPrefab activatingRootPrefab)
        {
            StartCoroutine(SwitchingPrefab(activatingRootPrefab));
        }

        private IEnumerator SwitchingPrefab(RootPrefab activatingRootPrefab)
        {
            if (_currentRootPrefab != null)
            {
                _switchRootPrefabPanel.Show();
                yield return new WaitUntil(() => _switchRootPrefabPanel.IsComplete);

                _currentRootPrefab.Deactivate();
            }

            _currentRootPrefab = activatingRootPrefab;

            _currentRootPrefab.Activate();

            yield return new WaitUntil(() => _currentRootPrefab.IsComplete);
            yield return new WaitForSeconds(0.5f);

            _switchRootPrefabPanel.Hide();

            yield return new WaitUntil(() => _switchRootPrefabPanel.IsComplete);

            _currentRootPrefab.ActivateInputSystem();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(RootPrefabController))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSwitchRootPrefabPanel(),
                DefineStartMenuRootPrefab(),
                DefineGameFieldRootPrefab()
            };

            return list;
        }

        [ContextMenu(nameof(DefineSwitchRootPrefabPanel))]
        private ComponentAttachInfo DefineSwitchRootPrefabPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _switchRootPrefabPanel, ComponentLocationTypes.InChildren);
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