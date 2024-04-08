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

        public IPlayable GetCard(CardCharacter cardCharacter)
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

        public void Add(CardCharacter cardCharacter, IPlayable card)
        {
            _playedCardPairs.Add(new PlayedCardPair(cardCharacter, card));
        }
    }
}
