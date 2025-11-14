using Cards;
using Cards.Effects;
using GameFields.Persons.EffectHandlers;
using Tools;
using Zenject;

namespace GameFields.Effects
{
    public class EffectData : IData
    {
        private readonly SignalBus _bus;
        private readonly CardEffectData _cardEffectData;
        private readonly EffectDuration _effectDuration;
        private readonly PersonEffectsHandlerRoot _personEffectsHandlerRoot;

        public EffectData(SignalBus bus, CardEffectData cardEffectData, EffectDuration effectDuration,
            PersonEffectsHandlerRoot personEffectsHandlerRoot)
        {
            _bus = bus;
            _cardEffectData = cardEffectData;
            _effectDuration = effectDuration;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
        }

        public SignalBus Bus => _bus;
        public CardEffectData CardEffectData => _cardEffectData;
        public EffectDuration EffectDuration => _effectDuration;
        public PersonEffectsHandlerRoot PersonEffectsHandlerRoot => _personEffectsHandlerRoot;
    }
}