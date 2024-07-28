using Cards;

namespace GameFields.Effects
{
    public abstract class Effect : ICardEffect
    {
        public readonly EffectTarget Target;

        protected Effect(EffectTarget target)
        {
            Target = target;
        }
    }
}
