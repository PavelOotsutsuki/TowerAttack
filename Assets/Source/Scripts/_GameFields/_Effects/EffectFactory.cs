using System;
using Cards;
using GameFields.Signals;
using Zenject;

namespace GameFields.Effects
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IPersonsState _personsState;
        //private readonly SignalBus _bus;

        public EffectFactory(IPersonsState personsState/*, SignalBus bus*/)
        {
            _personsState = personsState;
            //_bus = bus;
        }

        public Effect Create(CardEffectConfig effectConfig)
        {
            Effect effect = effectConfig.Type switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(_personsState.Deactive, effectConfig.Duration),
                EffectType.GreedyEffect => new GreedyEffect(_personsState.Active, _personsState.Deactive),
                EffectType.PyromancerEffect => new ZhyzhaEffect(_personsState.Deactive, effectConfig.Duration),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_personsState.Active, _personsState.Deactive),
                _ => throw new NullReferenceException("Effect is not founded")
            };

            _personsState.Active.StartEffect(effect);

            return effect;
        }
    }
}