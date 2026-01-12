using Cards;

namespace GameFields
{
    public interface IIndexTransitable : ITransitable
    {
        public void SeatCard(Card card, int index);
    }
}