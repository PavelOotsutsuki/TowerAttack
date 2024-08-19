using UnityEngine;
using System.Collections.Generic;

namespace GameFields.Seats
{
    public class SeatPool: MonoBehaviour
    {
        private const int CountExtraObjects = 10;

        [SerializeField] private Transform _container;
        [SerializeField] private int _countObjects;
        [SerializeField] private Seat _template;

        private Queue<Seat> _remainingPool = new Queue<Seat>();
        private List<Seat> _usedPool = new List<Seat>();

        public void Init()
        {
            for (int i = 0; i < _countObjects; i++)
            {
                CreateObject();
            }
        }

        public Seat GetHandSeat()
        {
            Seat result;

            if (_remainingPool.Count <= 0)
            {
                AddExtraObjects();
            }

            result = _remainingPool.Dequeue();
            result.gameObject.SetActive(true);
            _usedPool.Add(result);

            return result;
        }

        public void ReturnInPool(Seat handSeat)
        {
            if (_usedPool.Contains(handSeat))
            {
                handSeat.transform.SetParent(_container);
                handSeat.Reset();
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
            foreach (Seat handSeat in _usedPool)
            {
                _remainingPool.Enqueue(handSeat);
                handSeat.gameObject.SetActive(false);
            }

            _usedPool.Clear();
        }

        private void AddExtraObjects()
        {
            for (int i = 0; i < CountExtraObjects; i++)
            {
                CreateObject();
            }
        }

        private void CreateObject()
        {
            Seat spawned = Instantiate(_template, _container);
            spawned.Init();
            spawned.gameObject.SetActive(false);

            _remainingPool.Enqueue(spawned);
        }
    }
}
