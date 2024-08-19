using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Tables
{
    public class PlayedCards
    {
        private List<PlayedCardPair> _playedCardPairs;

        public PlayedCards()
        {
            _playedCardPairs = new List<PlayedCardPair>();
        }

        public Card GetCard(CardCharacter cardCharacter)
        {
            foreach (PlayedCardPair playedCardPair in _playedCardPairs)
            {
                if (playedCardPair.CardCharacter == cardCharacter)
                {
                    return playedCardPair.Card;
                }
            }

            return null;
        }

        //public List<Card> GetDiscardCards()
        //{
        //    List<Card> discardCards = new List<Card>();

        //    foreach (PlayedCardPair playedCardPair in _playedCardPairs)
        //    {
        //        playedCardPair.DecreaseCardCounter();

        //        if (playedCardPair.TryDiscard(out Card card))
        //        {
        //            discardCards.Add(card);
        //            _playedCardPairs.Remove(playedCardPair);
        //        }
        //    }

        //    return discardCards;
        //}

        public void Add(CardCharacter cardCharacter, Card card)
        {
            _playedCardPairs.Add(new PlayedCardPair(cardCharacter, card));
        }
    }
}
