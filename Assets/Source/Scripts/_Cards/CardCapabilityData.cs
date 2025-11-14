using Tools;

namespace Cards
{
    internal class CardCapabilityData : IData
    {
        private readonly string _toStringValue;
        private readonly string _description;
        private readonly string _colorTag;

        public CardCapabilityData(string toStringValue, string description, string colorTag)
        {
            _toStringValue = toStringValue;
            _description = description;
            _colorTag = colorTag;
        }

        public string ToStringValue => $"<b><color=#{_colorTag}>{_toStringValue}</color> </b>";
        public string Description => _description;
    }
}