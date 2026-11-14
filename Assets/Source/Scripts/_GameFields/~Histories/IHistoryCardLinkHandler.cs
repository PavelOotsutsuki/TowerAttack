using Cards.Views;

namespace GameFields.Histories
{
    public interface IHistoryCardLinkHandler
    {
        public CardViewData GetDataByLink(string link);
    }
}