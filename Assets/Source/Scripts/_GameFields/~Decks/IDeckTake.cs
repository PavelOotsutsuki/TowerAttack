using Cards;
using GameFields.CardTransits;

namespace GameFields.Decks
{
    public interface IDeckTake : ICardTakable, ICardCheck
    {
        public Card TakeTopCard();
        public int IndexOf(Card card);
    }
}