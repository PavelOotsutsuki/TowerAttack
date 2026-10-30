using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanelRootActivateData : IData
    {
        private IEnumerable<Card> _cards;

        public LookCardMenuSeatPanelRootActivateData(IEnumerable<Card> cards)
        {
            _cards = cards;
        }

        public IEnumerable<Card> Cards => _cards;
    }
}