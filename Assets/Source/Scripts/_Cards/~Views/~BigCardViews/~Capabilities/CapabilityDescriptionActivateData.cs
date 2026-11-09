using System.Threading;
using Tools;

namespace Cards.Views.BigCardViews.Capabilities
{
    public class CapabilityDescriptionActivateData : CancellationTokenData
    {
        private readonly CardCapability _cardCapability;

        public CapabilityDescriptionActivateData(CardCapability cardCapability, CancellationToken cardToken) : base(cardToken)
        {
            _cardCapability = cardCapability;
        }

        public CardCapability CardCapability => _cardCapability;
    }
}