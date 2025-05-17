using Cards;

namespace GameFields.Decks
{
    public interface IDeckTake: ICardCheck
    {
        public Card TakeTopCard();
        public Card TakeCard(Card card);
    }
}
