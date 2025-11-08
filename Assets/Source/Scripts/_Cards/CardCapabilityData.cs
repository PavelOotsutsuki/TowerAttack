using Tools;

namespace Cards
{
    public class CardCapabilityData : IData
    {
        //private readonly CardCapability _cardCapability;
        private readonly string _toStringValue;
        private readonly string _description;

        //public CardCapabilityData(CardCapability cardCapability, string toStringValue, string description)
        public CardCapabilityData(string toStringValue, string description)
        {
            //_cardCapability = cardCapability;
            _toStringValue = toStringValue;
            _description = description;
        }

        //public CardCapability CardCapability => _cardCapability;
        public string ToStringValue => _toStringValue;
        public string Description => _description;
    }
}