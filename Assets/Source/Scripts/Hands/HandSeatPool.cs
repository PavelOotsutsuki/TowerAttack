using UnityEngine;
using Pools;
using System.Collections.Generic;
using System.Linq;

namespace Hands
{
    public class HandSeatPool : ObjectPool
    {
        private List<HandSeat> _handSeats = new List<HandSeat>();

        protected override void ActivateObject(GameObject spawned)
        {
            if (spawned.TryGetComponent(out HandSeat handSeat))
            {
                handSeat.Init();
                _handSeats.Add(handSeat);
            }
        }

        public bool TryGetHandSeat(out HandSeat result)
        {
            result = _handSeats.FirstOrDefault(p => p.gameObject.activeSelf == false);

            if (result != default)
            {
                result.gameObject.SetActive(true);
            }

            return result != null;
        }
    }
}
