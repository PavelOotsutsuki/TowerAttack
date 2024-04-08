using Cards;

namespace GameFields.Persons.Tables
{
    public class PlayedCardPair
    {
        public PlayedCardPair(CardCharacter cardCharacter, IPlayable card)
        {
            CardCharacter = cardCharacter;
            Card = card;
        }

        public IPlayable Card { get; private set; }
        public CardCharacter CardCharacter { get; private set; }
    }
}
