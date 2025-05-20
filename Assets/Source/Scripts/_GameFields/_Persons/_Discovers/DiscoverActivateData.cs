using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.Discovers
{
    public class DiscoverActivateData : IData
    {
        private readonly IReadOnlyList<Card> _cards;
        private readonly string _activateMessage;
        private readonly DiscoverResult _discoverResult;

        public DiscoverActivateData(IReadOnlyList<Card> cards, string activateMessage, DiscoverResult discoverResult)
        {
            _cards = cards;
            _activateMessage = activateMessage;
            _discoverResult = discoverResult;
        }

        public IReadOnlyList<Card> Cards => _cards;
        public string ActivateMessage => _activateMessage;
        public DiscoverResult DiscoverResult => _discoverResult;
    }
}