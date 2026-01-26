using UnityEngine;

namespace Roots
{
    public class Creator : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameRoot _gameRootPrefab; 

        private void Start()
        {
            GameRoot gameRoot = Instantiate(_gameRootPrefab, _parent);
            gameRoot.gameObject.SetActive(true);
        }

    }
}
