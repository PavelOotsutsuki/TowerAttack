using Cards;

namespace GameFields.Persons.Hands
{
    public interface IHand
    {
        public void RemoveCard(Card card);
        public void AddCard(Card card);
    }
}
