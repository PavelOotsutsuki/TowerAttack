using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public abstract class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private int _countObjects;
        [SerializeField] private GameObject _template;

        private List<GameObject> _pool = new List<GameObject>();

        public void Init()
        {
            for (int i = 0; i < _countObjects; i++)
            {
                GameObject spawned = Instantiate(_template, _container.transform);
                ActivateObject(spawned);
                spawned.SetActive(false);

                _pool.Add(spawned);
            }
        }

        public void ReturnObjectInPool(GameObject returnableObject)
        {
            foreach (GameObject gameObject in _pool)
            {
                if (returnableObject == gameObject)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }

        public void ResetPool()
        {
            foreach (GameObject item in _pool)
            {
                item.SetActive(true);
                item.SetActive(false);
            }
        }

        abstract protected void ActivateObject(GameObject spawned);
    }
}
