using Tools;

namespace Cards
{
    internal class CardCapabilityData : IData
    {
        //private readonly CardCapability _cardCapability;
        private readonly string _toStringValue;
        private readonly string _description;
        private readonly string _colorTag;

        //public CardCapabilityData(CardCapability cardCapability, string toStringValue, string description)
        public CardCapabilityData(string toStringValue, string description, string colorTag)
        {
            //_cardCapability = cardCapability;
            _toStringValue = toStringValue;
            _description = description;
            _colorTag = colorTag;
        }

        //public CardCapability CardCapability => _cardCapability;
        public string ToStringValue => $"<b><color=#{_colorTag}>{_toStringValue}</color> </b>";
        //public string Description => $"<b><color=#{_colorTag}>{_toStringValue.ToUpper()}:</color> </b>{_description}";
        public string Description => $"</b>{_description}";
    }
}