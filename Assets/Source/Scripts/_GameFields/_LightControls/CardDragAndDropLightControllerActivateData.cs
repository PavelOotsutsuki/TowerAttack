using Cards;
using Tools;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightControllerActivateData : IData
    {
        private readonly CardCapability _effectFeature;

        public CardDragAndDropLightControllerActivateData(CardCapability effectFeature)
        {
            _effectFeature = effectFeature;
        }

        public CardCapability EffectFeature => _effectFeature;
    }
}