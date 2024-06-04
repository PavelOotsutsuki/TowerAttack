using System.Collections.Generic;
using System.Linq;
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
            return _playedCardPairs.First(pair => pair.CardCharacter == cardCharacter).Card;
        }

        public void Add(CardCharacter cardCharacter, Card card)
        {
            _playedCardPairs.Add(new PlayedCardPair(cardCharacter, card));
        }
    }
}
