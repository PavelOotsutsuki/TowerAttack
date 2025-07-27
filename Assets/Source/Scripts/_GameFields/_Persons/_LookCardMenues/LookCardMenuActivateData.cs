using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuActivateData : IData
    {
        private IEnumerable<Card> _cards;

        public LookCardMenuActivateData(IEnumerable<Card> cards)
        {
            _cards = cards;
        }

        public LookCardMenuSeatPanelRootActivateData LookCardMenuSeatPanelRootActivateData => new LookCardMenuSeatPanelRootActivateData(_cards);
    }
}