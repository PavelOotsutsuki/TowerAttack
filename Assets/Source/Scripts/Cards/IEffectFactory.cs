namespace Cards
{
    public interface IEffectFactory
    {
        public IEffect Create(EffectType type);
    }
}