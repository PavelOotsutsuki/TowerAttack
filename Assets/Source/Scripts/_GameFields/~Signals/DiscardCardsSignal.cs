using System.Collections.Generic;
using System.Threading;
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
        public readonly CancellationToken Token;

        public DiscardCardsSignal(Card card, CancellationToken token)
        {
            Card = card;
            Token = token;
        }
    }
}