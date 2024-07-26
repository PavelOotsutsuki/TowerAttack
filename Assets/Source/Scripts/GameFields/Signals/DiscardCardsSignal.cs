using System.Collections.Generic;
using Cards;

namespace GameFields.Discarding
{
    public struct DiscardCardsSignal
    {
        public readonly List<Card> Cards;

        public DiscardCardsSignal(List<Card> cards)
        {
            Cards = cards;
        }
    }
}