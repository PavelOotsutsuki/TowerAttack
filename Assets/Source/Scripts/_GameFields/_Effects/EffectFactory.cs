using System;
using Cards;
using GameFields.Signals;
using Zenject;

namespace GameFields.Effects
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IPersonsState _personsState;
        private readonly SignalBus _bus;

        public EffectFactory(IPersonsState personsState, SignalBus bus)
        {
            _personsState = personsState;
            _bus = bus;
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

            _personsState.Active.SetEffect(effect);
            //_bus.Fire(new StartEffectSignal(effect));

            return effect;
        }
    }
}