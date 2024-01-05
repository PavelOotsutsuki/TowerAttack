using UnityEngine;
using System.Collections.Generic;

namespace GameFields.Persons.Hands
{
    public class HandSeatPool : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private int _countObjects;
        [SerializeField] private HandSeat _template;

        private Queue<HandSeat> _remainingPool = new Queue<HandSeat>();
        private List<HandSeat> _usedPool = new List<HandSeat>();

        public void Init()
        {
            for (int i = 0; i < _countObjects; i++)
            {
                CreateObject();
            }
        }

        public bool TryGetHandSeat(out HandSeat result)
        {
            if (_remainingPool.Count <= 0)
            {
                result = null;
                return false;
            }

            result = _remainingPool.Dequeue();
            result.gameObject.SetActive(true);
            _usedPool.Add(result);
            return true;
        }

        public void ReturnInPool(HandSeat handSeat)
        {
            if (_usedPool.Contains(handSeat))
            {
                _usedPool.Remove(handSeat);
                _remainingPool.Enqueue(handSeat);
                handSeat.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("No contain on pool");
            }
        }

        public void ResetPool()
        {
            foreach (HandSeat handSeat in _usedPool)
            {
                _remainingPool.Enqueue(handSeat);
                handSeat.gameObject.SetActive(false);
            }

            _usedPool.Clear();
        }

        private void CreateObject()
        {
            HandSeat spawned = Instantiate(_template, _container);
            spawned.gameObject.SetActive(false);

            _remainingPool.Enqueue(spawned);
        }
    }
}
