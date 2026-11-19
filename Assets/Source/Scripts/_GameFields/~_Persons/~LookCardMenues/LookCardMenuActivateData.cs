using System.Collections.Generic;
using Cards;
using Tools;
using Tools.UI;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuActivateData : IData
    {
        private readonly IEnumerable<Card> _cards;
        private readonly LabelActivateData _labelActivateData;

        public LookCardMenuActivateData(IEnumerable<Card> cards, string message)
        {
            _cards = cards;
            _labelActivateData = new LabelActivateData(message);
        }

        public LookCardMenuSeatPanelRootActivateData LookCardMenuSeatPanelRootActivateData => new LookCardMenuSeatPanelRootActivateData(_cards);
        public IEnumerable<Card> Cards => _cards;
        public LabelActivateData LabelActivateData => _labelActivateData;
    }
}