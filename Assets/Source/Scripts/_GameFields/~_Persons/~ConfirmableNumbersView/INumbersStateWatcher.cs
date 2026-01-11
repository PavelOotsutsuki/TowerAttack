using System.Collections.Generic;

namespace GameFields.Persons.ConfirmableNumbersView
{
    public interface INumbersStateWatcher
    {
        public IEnumerable<int> FreeNumbers { get; }
        public IEnumerable<int> CheckedNumbers { get; }
    }
}