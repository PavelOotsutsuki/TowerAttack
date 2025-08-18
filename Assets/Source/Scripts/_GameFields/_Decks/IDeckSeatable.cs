using Cards;

namespace GameFields.Decks
{
    public interface IDeckSeatable
    {
        public void SeatCardWithoutShuffle(Card card);
    }
}