using Cards;

namespace GameFields.Persons.Tables
{
    public class PlayedCardContainer
    {
        public readonly Card Card;
        public readonly CardCharacter CardCharacter;
        
        public PlayedCardContainer(Card card, CardCharacter cardCharacter)
        {
            Card = card;
            CardCharacter = cardCharacter;
        }
    }
}