using Cards;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCardPair
    {
        public DiscoverCardPair(DiscoverCard discoverCard, Card card)
        {
            DiscoverCard = discoverCard;
            Card = card;
        }

        public Card Card { get; private set; }
        public DiscoverCard DiscoverCard { get; private set; }
    }
}
