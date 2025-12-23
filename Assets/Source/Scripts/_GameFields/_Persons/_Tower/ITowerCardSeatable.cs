using Cards;

namespace GameFields.Persons.Towers
{
    public interface ITowerCardSeatable : ICardDropPlace
    {
        public void SeatCard(Card card);
    }
}