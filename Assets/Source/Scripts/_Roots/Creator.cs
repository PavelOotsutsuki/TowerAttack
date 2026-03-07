using System.ComponentModel;
using StartMenues;
using Tools;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class Creator : MonoBehaviour, IActivatable, IDeactivatable
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _gameRootPrefab;
        [SerializeField] private StartMenuRoot _startMenuSceneRoot;
        //[SerializeField] private StartMenu _startMenu;
        [Inject] private DiContainer _diContainer;

        private GameFieldRoot _currentGameRoot;

        public void Activate()
        {
            GameObject gameRoot = _diContainer.InstantiatePrefab(_gameRootPrefab, _parent);
            //GameRoot gameRoot = Instantiate(_gameRootPrefab, _parent);
            //gameRoot.SetActive(true);
            _startMenuSceneRoot.gameObject.SetActive(false);

            _currentGameRoot = gameRoot.GetComponent<GameFieldRoot>();
        }

        public void Deactivate()
        {
            Destroy(_currentGameRoot.gameObject);
            _startMenuSceneRoot.gameObject.SetActive(true);
            _startMenuSceneRoot.Reactivate();
        }

    }
}