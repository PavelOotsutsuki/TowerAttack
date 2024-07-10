namespace Cards
{
    public struct PlayCardSignal
    {
        public readonly EffectType Effect;

        public PlayCardSignal(EffectType effect)
        {
            Effect = effect;
        }
    }
}