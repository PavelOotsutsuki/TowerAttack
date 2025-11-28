using Cards;
using Tools;

namespace GameFields.Persons.Fires
{
    public interface IFirePoolSeatable
    {
        public void SeatCard(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler);
    }
}