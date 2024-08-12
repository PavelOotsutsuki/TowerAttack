using System.Collections.Generic;
using Cards;

namespace GameFields.Discarding
{
    public struct DiscardCardsSignal
    {
        public readonly IEnumerable<Card> Cards;

        public DiscardCardsSignal(IEnumerable<Card> cards)
        {
            Cards = cards;
        }
    }
}