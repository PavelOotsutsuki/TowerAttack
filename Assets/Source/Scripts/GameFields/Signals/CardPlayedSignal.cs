using Cards;

namespace GameFields.Discarding
{
    public struct CardPlayedSignal
    {
        public readonly Card Card;

        public CardPlayedSignal(Card card)
        {
            Card = card;
        }
    }
}