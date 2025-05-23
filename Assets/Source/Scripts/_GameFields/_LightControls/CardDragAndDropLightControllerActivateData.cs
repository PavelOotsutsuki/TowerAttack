using Cards;
using Tools;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightControllerActivateData : IData
    {
        private readonly EffectFeature _effectFeature;

        public CardDragAndDropLightControllerActivateData(EffectFeature effectFeature)
        {
            _effectFeature = effectFeature;
        }

        public EffectFeature EffectFeature => _effectFeature;
    }
}