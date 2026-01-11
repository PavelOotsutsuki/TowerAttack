using Cards;

namespace GameFields
{
    public interface ICardTakable
    {
        public bool TryTakeAwayCard(Card card);
    }
}
