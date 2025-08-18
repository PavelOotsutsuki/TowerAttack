using Tools;
using UnityEngine;

namespace Cards
{
    public class OnFireLogicActivateData : IData
    {
        private readonly WaitForSeconds _delay;

        public OnFireLogicActivateData(WaitForSeconds delay)
        {
            _delay = delay;
        }

        public WaitForSeconds Delay => _delay;
    }
}