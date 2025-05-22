using GameFields.Persons.Common;

namespace GameFields
{
    public interface IPersonsState
    {
        public Person Active { get; }
        public Person Deactive { get; }
    }
}