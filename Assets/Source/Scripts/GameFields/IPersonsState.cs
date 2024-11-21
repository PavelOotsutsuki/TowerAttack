using System;
using GameFields.Persons;

namespace GameFields
{
    public interface IPersonsState
    {
        public Person Active { get; }
        public Person Deactive { get; }
    }
}
