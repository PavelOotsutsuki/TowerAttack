using System;
using Cards;

namespace GameFields.Effects
{
    public class EffectFactory
    {
        private readonly Deck _deck;

        public EffectFactory(Deck deck)
        {
            _deck = deck;
        }

        public Effect Create(EffectType type)
        {
            Effect effect = type switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(null),
                EffectType.GreedyEffect => new GreedyEffect(null,null),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_deck, null, null),
                _ => throw new NullReferenceException("Effect is not founded"),
            };

            return effect;
        }
    }
}