using System.Collections.Generic;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumbersList
    {
        private readonly List<ISelectNumber> _selectedNumbers;

        public SelectNumbersList()
        {
            _selectedNumbers = new List<ISelectNumber>();
        }

        public void Add(ISelectNumber selectNumber)
        {
            if (_selectedNumbers.Contains(selectNumber) == false)
                _selectedNumbers.Add(selectNumber);
        }

        public bool Contains(ISelectNumber selectNumber)
        {
            return _selectedNumbers.Contains(selectNumber);
        }

        public void Clear()
        {
            _selectedNumbers.Clear();
        }
    }
}