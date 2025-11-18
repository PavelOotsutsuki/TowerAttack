using Tools;

namespace Cards
{
    public class BigCardRootActivateData : IData
    {
        private readonly BigCardShowData _bigCardShowData;
        private readonly CardDescriptionActivateData _cardDescriptionShowData;
        private readonly CapabilityDescriptionActivateData _capabilityDescriptionShowData;

        public BigCardRootActivateData(BigCardShowData bigCardShowData = null,
            CardDescriptionActivateData cardDescriptionShowData = null,
            CapabilityDescriptionActivateData capabilityDescriptionShowData = null)
        {
            _bigCardShowData = bigCardShowData;
            _cardDescriptionShowData = cardDescriptionShowData;
            _capabilityDescriptionShowData = capabilityDescriptionShowData;
        }

        public bool CanActivateBigCard => _bigCardShowData != null;
        public bool CanActivateCardDescription => _cardDescriptionShowData != null;
        public bool CanActivateCapabilityDescription => _capabilityDescriptionShowData != null;

        public BigCardShowData BigCardShowData => _bigCardShowData;
        public CardDescriptionActivateData CardDescriptionShowData => _cardDescriptionShowData;
        public CapabilityDescriptionActivateData CapabilityDescriptionShowData => _capabilityDescriptionShowData;
    }
}