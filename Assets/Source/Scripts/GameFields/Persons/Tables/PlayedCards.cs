using System.Collections.Generic;
using System.Linq;
using Cards;

namespace GameFields.Persons.Tables
{
    public class PlayedCards
    {
        private readonly List<PlayedCardContainer> _played = new();

        public void Add(PlayedCardContainer played) 
            => _played.Add(played);

        public PlayedCardContainer RemoveByCard(Card card)
        {
            var played = _played.FirstOrDefault(p => p.Card == card);

            if (played == null)
                return null;
            
            _played.Remove(played);
            return played;
        }

        public PlayedCardContainer RemoveByCharacter(CardCharacter character)
        {
            var played = _played.FirstOrDefault(p => p.CardCharacter == character);

            if (played == null)
                return null;
            
            _played.Remove(played);
            return played;
        }
    }
}
