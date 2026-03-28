using Cards;
using Cards.Views;
using Tools;

namespace GameFields.Histories
{
    public class HistoryCardData : IData
    {
        private readonly bool _canView;
        private readonly CardViewData _cardViewData;

        public HistoryCardData(Card card)
        {
            _canView = card.IsVisibleByPlayer;

            _cardViewData = _canView ? card.ViewData : null;
        }

        public bool EqualsCardViewData(HistoryCardData historyCardData)
        {
            if (_cardViewData == null)
                return false;

            if (historyCardData == null)
                return false;

            return _cardViewData == historyCardData._cardViewData;
        }

        public bool CanView => _canView;
        public string Name => _cardViewData is null ? "?" : _cardViewData.Name;
        public string Number => _cardViewData is null ? "?" : _cardViewData.Number.ToString();
    }
}