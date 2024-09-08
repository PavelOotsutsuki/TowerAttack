namespace Cards
{
    public interface IEffectFactory
    {
        public Effect Create(EffectType type);
    }
}