using System;
using GameFields.Persons;

namespace GameFields
{
    public interface IPersonsState : IDisposable
    {
        public Person Active { get; }
        public Person Deactive { get; }
    }
}
