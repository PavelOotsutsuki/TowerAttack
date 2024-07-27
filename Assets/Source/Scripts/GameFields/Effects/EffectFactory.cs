using System;
using Cards;
using GameFields.Discarding;
using GameFields.Persons;
using Zenject;

namespace GameFields.Effects
{
    public class EffectFactory
    {
        private readonly SignalBus _bus;
        private readonly Deck _deck;
        private readonly IPersonsState _personsState;

        public EffectFactory(SignalBus bus, Deck deck, IPersonsState personsState)
        {
            _bus = bus;
            _deck = deck;
            _personsState = personsState;
            
            _bus.Subscribe<CardPlayedSignal>(OnCreateEffectSignal);
        }

        private void OnCreateEffectSignal(CardPlayedSignal signal) => Create(signal.Card);

        private void Create(Card card)
        {
            Effect effect = card.Effect switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(card, _personsState.ActivePerson),
                EffectType.GreedyEffect => new GreedyEffect(card, _personsState.ActivePerson, _personsState.DeactivePerson),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(card, _deck, _personsState.ActivePerson, _personsState.DeactivePerson),
                _ => throw new NullReferenceException("Effect is not founded"),
            };

            Person target = effect.Target == EffectTarget.Self
                ? _personsState.ActivePerson
                : _personsState.DeactivePerson;
            _bus.Fire(new EffectCreatedSignal(target, effect));
        }
    }
}