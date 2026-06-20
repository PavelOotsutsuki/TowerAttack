using System;
using Tools;
using UnityEngine;
using Zenject;

namespace Roots
{
    public abstract class RootPrefab : MonoBehaviour, IWorkable
    {
        [SerializeField] private GameObject _prefab;

        protected GameObject CurrentGameObject;

        private DiContainer _diContainer;
        private Container _container;

        private LocalRoot _localRoot;

        public bool? IsActive { get; private set; } = null;

        protected void Init(DiContainer diContainer, Container container)
        {
            IsActive = false;

            _diContainer = diContainer;
            _container = container;
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (CurrentGameObject == null)
            {
                CurrentGameObject = _diContainer.InstantiatePrefab(_prefab, _container.GetTransform());
                _localRoot = CurrentGameObject.GetComponent<LocalRoot>();
            }

            CurrentGameObject.SetActive(true);

            OnActivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            OnDeactivate();
        }

        public void ActivateInputSystem()
        {
            _localRoot.ActivateInputSystem();
        }
    }
}