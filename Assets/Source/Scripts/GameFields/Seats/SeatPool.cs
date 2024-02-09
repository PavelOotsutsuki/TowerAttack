using UnityEngine;
using System.Collections.Generic;

namespace GameFields.Seats
{
    public class SeatPool<T> : MonoBehaviour where T: Seat
    {
        [SerializeField] private Transform _container;
        [SerializeField] private int _countObjects;
        [SerializeField] private T _template;

        private Queue<T> _remainingPool = new Queue<T>();
        private List<T> _usedPool = new List<T>();

        public void Init()
        {
            for (int i = 0; i < _countObjects; i++)
            {
                CreateObject();
            }
        }

        public bool TryGetHandSeat(out T result)
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

        public void ReturnInPool(T handSeat)
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
            foreach (T handSeat in _usedPool)
            {
                _remainingPool.Enqueue(handSeat);
                handSeat.gameObject.SetActive(false);
            }

            _usedPool.Clear();
        }

        private void CreateObject()
        {
            T spawned = Instantiate(_template, _container);
            spawned.Init();
            spawned.gameObject.SetActive(false);

            _remainingPool.Enqueue(spawned);
        }
    }
}
