using System.Collections.Generic;
using Cards;

namespace GameFields.Signals
{
    public struct DiscardCardsSignal
    {
        //public readonly IEnumerable<Card> Cards;

        //public DiscardCardsSignal(IEnumerable<Card> cards)
        //{
        //    Cards = cards;
        //}

        public readonly Card Card;

        public DiscardCardsSignal(Card card)
        {
            Card = card;
        }
    }
}