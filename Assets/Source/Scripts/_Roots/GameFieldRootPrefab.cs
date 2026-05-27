using System;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Roots
{
    public class GameFieldRootPrefab : RootPrefab, IAutomaticFillComponents
    {
        [SerializeField] private GameFieldRootContainer _gameFieldRootContainer;

        private GameFieldRoot _currentGameFieldRoot;
        private Action _onDestroyPrefab;

        public void Init(DiContainer diContainer, Action onDestroyPrefab)
        {
            _onDestroyPrefab = onDestroyPrefab;

            base.Init(diContainer, _gameFieldRootContainer);
        }

        protected override void OnActivate()
        {
            _currentGameFieldRoot = CurrentGameObject.GetComponent<GameFieldRoot>();
            _currentGameFieldRoot.Init(_onDestroyPrefab);
            _currentGameFieldRoot.Activate();

            WaitingToComplete(this.destroyCancellationToken).Forget();
        }

        protected override void OnDeactivate()
        {
            Destroy(CurrentGameObject);
            CurrentGameObject = null;
            _currentGameFieldRoot = null;
        }

        private async UniTask WaitingToComplete(CancellationToken token)
        {
            await UniTask.Delay(500, cancellationToken: token);

            IsComplete = true;
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
            return AutomaticFillComponents.DefineComponent(this, ref _gameFieldRootContainer, ComponentLocationTypes.InThisElseChildren);
        }
        #endregion
    }
}