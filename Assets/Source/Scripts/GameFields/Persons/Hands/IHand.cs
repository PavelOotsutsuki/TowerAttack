using Cards;

namespace GameFields.Persons.Hands
{
    public interface IHand: IUnbindCardManager
    {
        public void AddCard(IHandSeatable seatableCard);
    }
}
