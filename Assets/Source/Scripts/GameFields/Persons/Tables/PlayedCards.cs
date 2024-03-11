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

        //public bool TryGetPlayedCardPair(out PlayedCardPair playedCardPair)
        //{
        //    if (_playedCardPairs.Count > 0)
        //    {
        //        playedCardPair = _playedCardPairs[0];
        //        _playedCardPairs.Remove(playedCardPair);

        //        return true;
        //    }

        //    playedCardPair = null;
        //    return false;
        //}

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

        public void Add(CardCharacter cardCharacter, Card card)
        {
            _playedCardPairs.Add(new PlayedCardPair(cardCharacter, card));
        }
    }
}
