namespace GameFields.Effects
{
    public abstract class Effect
    {
        public readonly EffectTarget Target;

        protected Effect(EffectTarget target)
        {
            Target = target;
        }
    }
}
