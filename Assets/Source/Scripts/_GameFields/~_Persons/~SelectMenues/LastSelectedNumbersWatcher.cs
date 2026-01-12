using System.Collections.Generic;

namespace GameFields.Persons.SelectMenues
{
    public class LastSelectedNumbersWatcher
    {
        private IEnumerable<int> _lastSelectedNumbers;

        public LastSelectedNumbersWatcher()
        {
            _lastSelectedNumbers = new List<int> { };
        }

        internal void SetNumbers(IEnumerable<int> numbers)
        {
            _lastSelectedNumbers = numbers;
        }

        public IEnumerable<int> LastSelectedNumbers => _lastSelectedNumbers;
    }
}