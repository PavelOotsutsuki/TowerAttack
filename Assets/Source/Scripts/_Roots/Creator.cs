using StartMenues;
using Tools;
using UnityEngine;

namespace Roots
{
    public class Creator : MonoBehaviour, IActivatable, IDeactivatable
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameRoot _gameRootPrefab;
        [SerializeField] private StartMenuSceneRoot _startMenuSceneRoot;
        [SerializeField] private StartMenu _startMenu;

        private GameRoot _currentGameRoot;

        public void Activate()
        {
            GameRoot gameRoot = Instantiate(_gameRootPrefab, _parent);
            gameRoot.gameObject.SetActive(true);
            _startMenuSceneRoot.gameObject.SetActive(false);

            _currentGameRoot = gameRoot;
        }

        public void Deactivate()
        {
            Destroy(_currentGameRoot.gameObject);
            _startMenuSceneRoot.gameObject.SetActive(true);
            _startMenuSceneRoot.Reactivate();
        }

    }
}