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
            return true;
        }

        public void ReturnInPool(HandSeat handSeat)
        {
            _remainingPool.Enqueue(handSeat);
            handSeat.gameObject.SetActive(true);
        }

        public void ResetPool()
        {
            foreach (HandSeat handSeat in _remainingPool)
            {
                handSeat.gameObject.SetActive(false);
            }

            while (_remainingPool.Count < _countObjects)
            {
                CreateObject();
            }
        }

        private void CreateObject()
        {
            HandSeat spawned = Instantiate(_template, _container);
            spawned.gameObject.SetActive(false);

            _remainingPool.Enqueue(spawned);
        }
    }
}
