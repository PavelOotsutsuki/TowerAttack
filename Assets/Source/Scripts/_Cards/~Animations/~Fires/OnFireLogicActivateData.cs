using System.Threading;
using Tools;

namespace Cards.Animations.Fires
{
    internal class OnFireLogicActivateData : CancellationTokenData
    {
        private readonly CallbackHandler _callbackHandler;
        private readonly float _delay;

        public OnFireLogicActivateData(float delay, CallbackHandler callbackHandler, CancellationToken token) : base(token)
        {
            _callbackHandler = callbackHandler;
            _delay = delay;
        }

        public CallbackHandler CallbackHandler => _callbackHandler;
        public float Delay => _delay;
    }
}