using GameFields.Persons.Commons;

namespace GameFields
{
    public interface IPersonsState
    {
        public Person Active { get; }
        public Person Deactive { get; }
    }
}