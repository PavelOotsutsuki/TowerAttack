using System.Collections.Generic;

namespace GameFields.Persons.Tables
{
    public class PlayedCards
    {
        private readonly List<PlayedCardContainer> _played = new();

        public void Add(PlayedCardContainer played) 
            => _played.Add(played);

        public List<PlayedCardContainer> GetDiscardCards()
        {
            List<PlayedCardContainer> discarded = new();
            var pairs2Temp = new List<PlayedCardContainer>(_played);

            foreach (PlayedCardContainer container in pairs2Temp)
            {
                if (container.Effect.CountTurns < 1)
                {
                    discarded.Add(container);
                    _played.Remove(container);
                }
            }
            
            return discarded;
        }
    }
}
