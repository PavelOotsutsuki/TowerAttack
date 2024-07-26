using GameFields.Persons;

namespace GameFields
{
    public interface IPersonSides
    {
        public Person ActivePerson { get; }
        public Person DeactivePerson { get; }
    }
}
