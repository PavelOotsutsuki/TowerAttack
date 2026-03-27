using Tools;

namespace Cards.Views.BigCardViews.Capabilities
{
    public class CapabilityDescriptionActivateData : IData
    {
        private readonly CardCapability _cardCapability;

        public CapabilityDescriptionActivateData(CardCapability cardCapability)
        {
            _cardCapability = cardCapability;
        }

        public CardCapability CardCapability => _cardCapability;
    }
}