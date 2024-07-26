using Cards;
using GameFields.Effects;

namespace GameFields.Persons.Tables
{
    public class PlayedCardContainer
    {
        public readonly Card Card;
        public readonly CardCharacter CardCharacter;
        public readonly Effect Effect;
        
        public PlayedCardContainer(Card card, CardCharacter cardCharacter, Effect effect)
        {
            Card = card;
            CardCharacter = cardCharacter;
            Effect = effect;
        }
    }
}