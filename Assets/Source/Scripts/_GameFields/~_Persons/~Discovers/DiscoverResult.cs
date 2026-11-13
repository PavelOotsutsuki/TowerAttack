using System;
using Cards;
using Cards.Views;

namespace GameFields.Persons.Discovers
{
    public class DiscoverResult
    {
        private readonly Action<IDiscoverable> _callbackAfterSetResult;
        private readonly Action<IDiscoverable> _callbackAfterSetComplete;

        public DiscoverResult(Action<IDiscoverable> callbackAfterSetResult = null, Action<IDiscoverable> callbackAfterSetComplete = null)
        {
            Result = null;
            IsComplete = false;

            _callbackAfterSetResult = callbackAfterSetResult;
            _callbackAfterSetComplete = callbackAfterSetComplete;
        }

        public IDiscoverable Result { get; private set; }
        public bool IsComplete { get; private set; }

        public void SetResult(IDiscoverable result)
        {
            Result = result;
            _callbackAfterSetResult?.Invoke(Result);
        }

        public void SetComplete()
        {
            if (Result == null)
                throw new Exception("Нельзя задать complete до того как задали result!");

            IsComplete = true;
            _callbackAfterSetComplete?.Invoke(Result);
        }
    }
}