using Tools;
using UnityEngine;

namespace Cards.Animations.Fires
{
    internal class OnFireLogicActivateData : IData
    {
        private readonly CallbackHandler _callbackHandler;
        private readonly WaitForSeconds _delay;

        public OnFireLogicActivateData(WaitForSeconds delay, CallbackHandler callbackHandler)
        {
            _callbackHandler = callbackHandler;
            _delay = delay;
        }

        public CallbackHandler CallbackHandler => _callbackHandler;
        public WaitForSeconds Delay => _delay;
    }
}