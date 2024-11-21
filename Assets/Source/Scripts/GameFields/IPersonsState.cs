using GameFields.Persons;

namespace GameFields
{
    public interface IPersonsState
    {
        public Person ActivePerson { get; }
        public Person DeactivePerson { get; }
    }
}
