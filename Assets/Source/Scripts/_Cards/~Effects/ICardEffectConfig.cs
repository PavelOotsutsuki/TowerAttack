using Tools;

namespace Cards.Effects
{
    public interface ICardEffectConfig : IData
    {
        public EffectType Type { get; }
        public int Duration { get; }
    }
}