using Cards;

namespace GameFields.Decks
{
    public interface IDeckTake : ICardTakable, ICardCheck
    {
        public Card TakeTopCard();
    }
}
