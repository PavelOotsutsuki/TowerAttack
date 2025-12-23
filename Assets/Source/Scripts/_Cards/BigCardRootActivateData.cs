using Tools;

namespace Cards
{
    public class BigCardRootActivateData : IData
    {
        private readonly BigCardShowData _bigCardShowData;
        private readonly CardCapability _cardCapability;

        public BigCardRootActivateData(BigCardShowData bigCardShowData, CardCapability cardCapability)
        {
            _bigCardShowData = bigCardShowData;
            _cardCapability = cardCapability;
        }

        public BigCardShowData BigCardShowData => _bigCardShowData;
        public CardCapability CardCapability => _cardCapability;
    }
}