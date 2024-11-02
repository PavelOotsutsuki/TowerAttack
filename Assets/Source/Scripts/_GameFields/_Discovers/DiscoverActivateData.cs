using System;
using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.Discovers
{
    public class DiscoverActivateData : IData
    {
        private readonly List<Card> _cards;
        private readonly string _activateMessage;
        private readonly Action<Card> _callback;

        public DiscoverActivateData(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            _cards = cards;
            _activateMessage = activateMessage;
            _callback = callback;
        }

        public IReadOnlyList<Card> Cards => _cards;
        public string ActivateMessage => _activateMessage;
        public Action<Card> Callback => _callback;
    }
}