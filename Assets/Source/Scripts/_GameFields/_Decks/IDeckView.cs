using Cards;

namespace GameFields.Decks
{
    public interface IDeckView: ICardView
    {
        public Card ViewCardFromEndDeck(int index = 0);
        public Card ViewCardFromTopDeck(int index = 0);
    }
}