using Cards;
using Tools;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightControllerActivateData : IData
    {
        private readonly EffectType _effectType;

        public CardDragAndDropLightControllerActivateData(EffectType effectType)
        {
            _effectType = effectType;
        }

        public EffectType EffectType => _effectType;
    }
}