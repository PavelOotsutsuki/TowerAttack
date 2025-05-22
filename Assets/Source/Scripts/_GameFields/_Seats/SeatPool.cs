using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace GameFields.Seats
{
    public class SeatPool: MonoBehaviour
    {
        private const int CountExtraObjects = 500;

        [SerializeField] private Transform _container;
        [SerializeField] private int _countObjects;
        [SerializeField] private Seat _template;

        private readonly Queue<Seat> _remainingPool = new Queue<Seat>();
        private readonly List<Seat> _usedPool = new List<Seat>();

        //private readonly List<Seat> _returnablePool = new List<Seat>();

        public void Init()
        {
            for (int i = 0; i < _countObjects; i++)
            {
                CreateObject();
            }
        }

        public Seat GetSeat()
        {
            if (_remainingPool.Count <= 0)
            {
                AddExtraObjects();
            }

            Seat result = _remainingPool.Dequeue();
            result.gameObject.SetActive(true);
            _usedPool.Add(result);

            //if (_returnablePool.Contains(result))
            //    _returnablePool.Remove(result);

            return result;
        }

        public void ReturnInPool(Seat handSeat)
        {
            if (_usedPool.Contains(handSeat))
            {
                handSeat.ReadOnlyTransform.SetParent(_container);
                //_returnablePool.Add(handSeat);
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

        //public void Collect()
        //{
        //    if (_returnablePool.Count == 0)
        //        return;

        //    foreach (Seat seat in _returnablePool)
        //    {
        //        seat.ReadOnlyTransform.SetParent(_container);
        //    }

        //    _returnablePool.Clear();
        //}

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