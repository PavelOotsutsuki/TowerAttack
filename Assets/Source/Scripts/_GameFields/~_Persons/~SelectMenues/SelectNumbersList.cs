using System.Collections.Generic;

namespace GameFields.Persons.SelectMenues
{
    public class SelectNumbersList
    {
        private readonly Dictionary<int, NumberAnimationType> _selectedNumbersStates;

        public IReadOnlyDictionary<int, NumberAnimationType> SelectedNumbersStates => _selectedNumbersStates;

        public SelectNumbersList()
        {
            _selectedNumbersStates = new Dictionary<int, NumberAnimationType>();
        }

        public void Add(int selectNumber, NumberAnimationType type)
        {
            if (_selectedNumbersStates.ContainsKey(selectNumber) == false)
                _selectedNumbersStates.Add(selectNumber, type);
        }

        public bool Contains(int selectNumber)
        {
            return _selectedNumbersStates.ContainsKey(selectNumber);
        }

        public void Clear()
        {
            _selectedNumbersStates.Clear();
        }

        public NumberAnimationType GetType(int selectNumber)
        {
            return _selectedNumbersStates[selectNumber];
        }
    }
}