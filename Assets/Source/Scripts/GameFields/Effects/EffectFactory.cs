using System;
using Cards;
using GameFields.Persons;
using Zenject;

namespace GameFields.Effects
{
    public class EffectFactory : IEffectFactory
    {
        //private readonly SignalBus _bus;
        //private readonly Deck _deck;
        private readonly IPersonsState _personsState;

        public EffectFactory(IPersonsState personsState)
        {
            //_bus = bus;
            //_deck = deck;
            _personsState = personsState;
        }

        public Effect Create(EffectType type)
        {
            Effect effect = type switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(_personsState.Active),
                EffectType.GreedyEffect => new GreedyEffect(_personsState.Active, _personsState.Deactive),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_personsState.Active, _personsState.Deactive),
                _ => throw new NullReferenceException("Effect is not founded"),
            };

            return effect;
            //Person target = effect.Target == EffectTarget.Self
            //    ? _personsState.Active
            //    : _personsState.Deactive;
            
            //_bus.Fire(new EffectCreatedSignal(target, type));
        }
    }
}