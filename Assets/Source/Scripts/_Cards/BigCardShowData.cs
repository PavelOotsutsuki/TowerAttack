using Tools;

namespace Cards
{
    public class BigCardShowData : IData
    {
        private readonly CardViewData _cardViewData;

        public BigCardShowData(CardViewData cardViewData)
        {
            _cardViewData = cardViewData;
        }

        public CardViewData CardViewData => _cardViewData;
    }
}