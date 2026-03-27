using System;
using System.Collections.Generic;
using System.Linq;
using GameFields.Persons;
using Random = UnityEngine.Random;

namespace GameFields.Persons.SelectMenues
{
    public class DefaultRandomSelectNumberLogic : IRandomSelectNumberLogic
    {
        private readonly int _needForActivate;
        //private readonly IEnumerable<ISelectNumber> _shuffleNumbers;
        private readonly IEnumerable<ISelectNumber> _currentAvailableNumbers;
        private readonly IEnumerable<ISelectNumber> _shuffleAvailableNumbers;
        //private readonly ConfirmableNumbers _confirmableNumbers;

        public DefaultRandomSelectNumberLogic(int needForActivate, IEnumerable<ISelectNumber> currentAvailableNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _needForActivate = needForActivate;
            _currentAvailableNumbers = currentAvailableNumbers;
            //_confirmableNumbers = confirmableNumbers;

            _shuffleAvailableNumbers = Shuffle(currentAvailableNumbers).Where(e => confirmableNumbers.Contains(e.Number) == false);
            //_shuffleNumbers = Shuffle(currentAvailableNumbers);
        }

        public IReadOnlyList<ISelectNumber> GetSelectedNumbers()
        {
            List<ISelectNumber> selectedNumbers = new List<ISelectNumber>();

            for (int i = 0; i < _needForActivate; i++)
            {
                ISelectNumber selectedNumber = GetSelectedNumber(selectedNumbers) ?? throw new Exception("Ошибка нахождения номера для имитации select-a");
                selectedNumbers.Add(selectedNumber);
            }

            return selectedNumbers;
        }

        private ISelectNumber GetSelectedNumber(IReadOnlyList<ISelectNumber> alreadySelectedNumbers)
        {
            if (_shuffleAvailableNumbers.Count() == 1)
            {
                if (alreadySelectedNumbers.Contains(_shuffleAvailableNumbers.First()) == false)
                    return _shuffleAvailableNumbers.First();
            }

            if (alreadySelectedNumbers.Count + 1 >= _shuffleAvailableNumbers.Count())
            {
                foreach (ISelectNumber number in _currentAvailableNumbers)
                {
                    if (_shuffleAvailableNumbers.Contains(number) == false && alreadySelectedNumbers.Contains(number) == false)
                        return number;
                }
            }

            foreach (ISelectNumber number in _shuffleAvailableNumbers)
            {
                if (alreadySelectedNumbers.Contains(number) == false)
                {
                    return number;
                }
            }

            return null;
        }

        private IEnumerable<ISelectNumber> Shuffle(IEnumerable<ISelectNumber> shuffledList)
        {
            List<ISelectNumber> shuffleNumbers = new List<ISelectNumber>();
            List<ISelectNumber> allNumbers = new List<ISelectNumber>();

            foreach (ISelectNumber selectNumber in shuffledList)
            {
                allNumbers.Add(selectNumber);
            }

            while (allNumbers.Count > 0)
            {
                ISelectNumber selectNumber = allNumbers[Random.Range(0, allNumbers.Count)];
                shuffleNumbers.Add(selectNumber);
                allNumbers.Remove(selectNumber);
            }

            return shuffleNumbers;
        }
    }
}