using Cards;
using Tools;
using Zenject;

namespace GameFields.Effects
{
    public class EffectData : IData
    {
        private readonly SignalBus _bus;
        private readonly CardEffectData _cardEffectData;
        private readonly EffectDuration _effectDuration;

        public EffectData(SignalBus bus, CardEffectData cardEffectData, EffectDuration effectDuration)
        {
            _bus = bus;
            _cardEffectData = cardEffectData;
            _effectDuration = effectDuration;
        }

        public SignalBus Bus => _bus;
        public CardEffectData CardEffectData => _cardEffectData;
        public EffectDuration EffectDuration => _effectDuration;
    }
}