using Cards;

namespace GameFields
{
    public interface ICardSeatable
    { 
        public void SeatCard(Card card, int index = -1);
    }
}