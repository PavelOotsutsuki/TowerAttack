using GameFields.Persons;

namespace GameFields
{
    public interface IPersonSideListener
    {
        public IPerson ActivePerson { get; }
        public IPerson DeactivePerson { get; }
    }
}
