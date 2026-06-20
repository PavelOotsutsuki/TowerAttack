using Cards.Effects;
using GameFields.Histories;
using GameFields.Persons;
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
        private readonly HistoryRoot _historyRoot;
        private readonly Person _activePerson;

        public EffectData(SignalBus bus, CardEffectData cardEffectData, EffectDuration effectDuration,
            PersonEffectsHandlerRoot personEffectsHandlerRoot, HistoryRoot historyRoot, Person activePerson)
        {
            _bus = bus;
            _cardEffectData = cardEffectData;
            _effectDuration = effectDuration;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
            _historyRoot = historyRoot;
            _activePerson = activePerson;
        }

        public SignalBus Bus => _bus;
        public CardEffectData CardEffectData => _cardEffectData;
        public EffectDuration EffectDuration => _effectDuration;
        public PersonEffectsHandlerRoot PersonEffectsHandlerRoot => _personEffectsHandlerRoot;
        public HistoryRoot HistoryRoot => _historyRoot;
        public Person ActivePerson => _activePerson;
    }
}