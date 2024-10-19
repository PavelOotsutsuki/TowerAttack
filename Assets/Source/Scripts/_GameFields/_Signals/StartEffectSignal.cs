using Cards;

namespace GameFields.Signals
{
    public struct StartEffectSignal
    {
        public readonly Card Card;

        public StartEffectSignal(Card card)
        {
            Card = card;
        }
    }
}