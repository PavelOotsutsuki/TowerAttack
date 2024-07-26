namespace GameFields.Effects
{
    public abstract class Effect
    {
        public EffectTarget Target { get; }
        public int CountTurns { get; protected set; }

        protected Effect(EffectTarget target)
        {
            Target = target;
        }

        public void DecreaseCounter()
        {
            CountTurns -= 1;
        }
    }
}
