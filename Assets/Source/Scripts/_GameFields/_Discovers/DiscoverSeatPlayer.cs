using Cards;

namespace GameFields.Persons.Discovers
{
    public class DiscoverSeatPlayer : DiscoverSeat
    {
        public override void SetCard(Card card)
        {
            Card = card;
            DiscoverCard.Activate(Card.Transform.sizeDelta.y, Card.Transform.sizeDelta.x, Card.ViewConfig);
            //DiscoverCard.Activate(Card.ViewConfig);
        }
    }
}
