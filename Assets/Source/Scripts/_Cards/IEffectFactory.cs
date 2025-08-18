namespace Cards
{
    public interface IEffectFactory
    {
        public Effect Create(CardEffectConfig effectConfig);
    }
}