using Cards;

namespace GameFields.CardTransits
{
    public interface ICardTakable
    {
        public bool TryTakeAwayCard(Card card);
    }
}
