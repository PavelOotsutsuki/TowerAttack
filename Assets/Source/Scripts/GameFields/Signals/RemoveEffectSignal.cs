using Cards;

namespace GameFields.Discarding
{
    public struct RemoveEffectSignal
    {
        public readonly EffectType Type;

        public RemoveEffectSignal(EffectType type)
        {
            Type = type;
        }
    }
}