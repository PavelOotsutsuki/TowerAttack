using Cards;

namespace GameFields.Decks
{
    public interface IDeckView: ICardView
    {
        public Card ViewCardFromEndDeck(int index = 0);
    }
}