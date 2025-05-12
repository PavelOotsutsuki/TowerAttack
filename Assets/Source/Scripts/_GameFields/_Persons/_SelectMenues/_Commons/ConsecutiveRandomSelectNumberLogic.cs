using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class ConsecutiveRandomSelectNumberLogic : IRandomSelectNumberLogic
    {
        private readonly int _needForActivate;
        //private readonly ConfirmableNumbers _confirmableNumbers;
        private readonly IEnumerable<ISelectNumber> _currentAvailableNumbers;
        private readonly IEnumerable<ISelectNumber> _shuffleAvailableNumbers;

        private readonly int _maxNumber;

        public ConsecutiveRandomSelectNumberLogic(int needForActivate, IReadOnlyList<ISelectNumber> currentAvailableNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _needForActivate = needForActivate;
            //_confirmableNumbers = confirmableNumbers;
            _currentAvailableNumbers = currentAvailableNumbers;

            _maxNumber = currentAvailableNumbers.Select(e => e.Number).Max();

            _shuffleAvailableNumbers = Shuffle(currentAvailableNumbers).Where(e => confirmableNumbers.Contains(e.Number) == false);
            //_maxNumber = currentAvailableNumbers.Max(e => e.Number);
        }

        public IReadOnlyList<ISelectNumber> GetSelectedNumbers()
        {
            IReadOnlyList<ISelectNumber> selectedNumbers;

            ISelectNumber firstNumber = GetFirstNumber() ?? throw new Exception("Ошибка нахождения номера для имитации select-a");

            selectedNumbers = FillResult(firstNumber);

            return selectedNumbers;
        }

        private ISelectNumber GetFirstNumber()
        {
            IEnumerable<int> shuffleAvailableNumbersInt = _shuffleAvailableNumbers.Select(e => e.Number);

            foreach (ISelectNumber number in _shuffleAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                    continue;

                //if (_confirmableNumbers.Contains(number.Number))
                //    continue;

                int i = 1;

                while (i < _needForActivate)
                {
                    if (shuffleAvailableNumbersInt.Contains(number.Number + i) == false)
                        break;

                    i++;
                }

                if (i == _needForActivate)
                {
                    if (_shuffleAvailableNumbers.Count() == _needForActivate)
                    {
                        return _currentAvailableNumbers.Where(e => e.Number == number.Number + 1).First();
                    }

                    return number;
                }
            }

            foreach (ISelectNumber number in _shuffleAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                {
                    return _currentAvailableNumbers.Where(e => e.Number == _maxNumber - _needForActivate + 1).First();
                }

                //if (_confirmableNumbers.Contains(number.Number))
                //    continue;

                return number;
            }

            foreach (ISelectNumber number in _currentAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                    continue;

                //IReadOnlyList<ISelectNumber> preliminaryResult = FillResult(number);

                //foreach (ISelectNumber selectNumber in _shuffleAvailableNumbers)
                //{
                //    if (preliminaryResult.Contains(selectNumber) == false)
                //    {
                //        return number;
                //    }
                //}

                return number;
            }

            return null;
        }

        private IReadOnlyList<ISelectNumber> FillResult(ISelectNumber firstNumber)
        {
            List<ISelectNumber> selectedNumbers = new List<ISelectNumber>();

            selectedNumbers.Add(firstNumber);

            for (int i = 1; i < _needForActivate; i++)
            {
                ISelectNumber selectedNumber = _currentAvailableNumbers.Where(e => e.Number == firstNumber.Number + i).First();
                selectedNumbers.Add(selectedNumber);
            }

            return selectedNumbers;
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