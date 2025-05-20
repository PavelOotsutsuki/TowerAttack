using System;
using Cards;

namespace GameFields.Persons.Discovers
{
    public class DiscoverResult
    {
        private readonly Action<Card> _callbackAfterSetResult;
        private readonly Action<Card> _callbackAfterSetComplete;

        public DiscoverResult(Action<Card> callbackAfterSetResult = null, Action<Card> callbackAfterSetComplete = null)
        {
            Result = null;
            IsComplete = false;

            _callbackAfterSetResult = callbackAfterSetResult;
            _callbackAfterSetComplete = callbackAfterSetComplete;
        }

        public Card Result { get; private set; }
        public bool IsComplete { get; private set; }

        public void SetResult(Card result)
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