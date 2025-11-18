using System.Collections.Generic;

namespace GameFields.Persons
{
    public interface INumbersStateWatcher
    {
        public IEnumerable<int> FreeNumbers { get; }
        public IEnumerable<int> CheckedNumbers { get; }
    }
}