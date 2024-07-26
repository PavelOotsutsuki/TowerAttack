using GameFields.Persons;

namespace GameFields
{
    public interface IPersonSideListener
    {
        public Person ActivePerson { get; }
        public Person DeactivePerson { get; }
    }
}
