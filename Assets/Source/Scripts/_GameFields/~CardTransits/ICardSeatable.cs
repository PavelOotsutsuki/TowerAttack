using Cards;

namespace GameFields.CardTransits
{
    public interface ICardSeatable
    { 
        public void SeatCard(Card card, int index = -1);
    }
}