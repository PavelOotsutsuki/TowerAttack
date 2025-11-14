using Cards;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public interface IAttackCardKeeper
    {
        public Card SeizeAttackingCard { get; }
    }
}