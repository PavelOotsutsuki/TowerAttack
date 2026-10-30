using System.Threading;
using Cards.Effects;
using GameFields.Histories;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers;
using Servers;
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
        private readonly CancellationToken _fightToken;
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly EffectType _playedEffectType;

        public EffectData(SignalBus bus, CardEffectData cardEffectData, EffectDuration effectDuration,
            PersonEffectsHandlerRoot personEffectsHandlerRoot, HistoryRoot historyRoot, Person activePerson,
            CancellationToken fightToken, FightProcessDBManager fightProcessDBManager, EffectType playedEffectType)
        {
            _bus = bus;
            _cardEffectData = cardEffectData;
            _effectDuration = effectDuration;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
            _historyRoot = historyRoot;
            _activePerson = activePerson;
            _fightToken = fightToken;
            _fightProcessDBManager = fightProcessDBManager;
            _playedEffectType = playedEffectType;
        }

        public SignalBus Bus => _bus;
        public CardEffectData CardEffectData => _cardEffectData;
        public EffectDuration EffectDuration => _effectDuration;
        public PersonEffectsHandlerRoot PersonEffectsHandlerRoot => _personEffectsHandlerRoot;
        public HistoryRoot HistoryRoot => _historyRoot;
        public Person ActivePerson => _activePerson;
        public CancellationToken FightToken => _fightToken;
        public FightProcessDBManager FightProcessDBManager => _fightProcessDBManager;
        public EffectType PlayedEffectType => _playedEffectType;
    }
}