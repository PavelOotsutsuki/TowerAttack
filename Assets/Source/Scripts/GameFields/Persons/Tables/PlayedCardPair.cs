using Cards;

namespace GameFields
{
    public class PlayedCardPair
    {
        public PlayedCardPair(CardCharacter cardCharacter, Card card)
        {
            CardCharacter = cardCharacter;
            Card = card;
        }

        public Card Card { get; private set; }
        public CardCharacter CardCharacter { get; private set; }
    }
}
