using GameFields.Effects;

namespace GameFields.Discarding
{
    public struct EffectCreatedSignal
    {
        public readonly Effect Effect;

        public EffectCreatedSignal(Effect effect)
        {
            Effect = effect;
        }
    }
}