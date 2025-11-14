using Cards;

namespace GameFields.Persons.Tables
{
    public interface IDiscardManager
    {
        public void Discard(Card card);
        public bool HasCard(Card card);
    }
}