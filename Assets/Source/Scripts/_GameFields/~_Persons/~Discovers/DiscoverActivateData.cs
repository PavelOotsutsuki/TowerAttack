using System.Collections.Generic;
using Cards;
using Cards.Views;
using Tools;

namespace GameFields.Persons.Discovers
{
    public class DiscoverActivateData : IData
    {
        private readonly IReadOnlyList<IDiscoverable> _cards;
        private readonly string _activateMessage;
        private readonly DiscoverResult _discoverResult;

        public DiscoverActivateData(IReadOnlyList<IDiscoverable> cards, string activateMessage, DiscoverResult discoverResult)
        {
            _cards = cards;
            _activateMessage = activateMessage;
            _discoverResult = discoverResult;
        }

        public IReadOnlyList<IDiscoverable> Cards => _cards;
        public string ActivateMessage => _activateMessage;
        public DiscoverResult DiscoverResult => _discoverResult;
    }
}