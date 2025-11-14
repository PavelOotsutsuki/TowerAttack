using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanelActivateData : IData
    {
        private IEnumerable<Card> _cards;

        public LookCardMenuSeatPanelActivateData(IEnumerable<Card> cards)
        {
            _cards = cards;
        }

        public IEnumerable<Card> Cards => _cards;
    }
}