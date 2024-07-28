using GameFields.Effects;

namespace GameFields.Discarding
{
    public struct RemoveEffectSignal
    {
        public readonly Effect Effect;

        public RemoveEffectSignal(Effect effect)
        {
            Effect = effect;
        }
    }
}