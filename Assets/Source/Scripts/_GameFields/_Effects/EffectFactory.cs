using System;
using Cards;

namespace GameFields.Effects
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IPersonsState _personsState;

        public EffectFactory(IPersonsState personsState)
        {
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
        }
    }
}