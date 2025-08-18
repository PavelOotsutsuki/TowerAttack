using System;
using System.Collections.Generic;
using System.Linq;
using GameFields.Persons.Commons;
using Random = UnityEngine.Random;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class ConsecutiveRandomSelectNumberLogic : IRandomSelectNumberLogic
    {
        private readonly int _needForActivate;
        private readonly IEnumerable<ISelectNumber> _currentAvailableNumbers;
        private readonly IEnumerable<ISelectNumber> _shuffleAvailableNumbers;

        private readonly int _maxNumber;
        private readonly int _minNumber;

        public ConsecutiveRandomSelectNumberLogic(int needForActivate, IEnumerable<ISelectNumber> currentAvailableNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _needForActivate = needForActivate;
            _currentAvailableNumbers = currentAvailableNumbers;

            _maxNumber = currentAvailableNumbers.Select(e => e.Number).Max();
            _minNumber = currentAvailableNumbers.Select(e => e.Number).Min();

            _shuffleAvailableNumbers = Shuffle(currentAvailableNumbers).Where(e => confirmableNumbers.Contains(e.Number) == false);
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

            // Ищем совпадния по 4 (_needForActivate) номерам сразу 
            foreach (ISelectNumber number in _shuffleAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                    continue;

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
                        // Совпадения нашли, но номеров осталось всего 4.
                        // Проверять каждый раз одни и те же бессмысленно, поэтому берем (number + 1) или (number - 1) 
                        int findedNumber = number.Number;
                        int vector = Random.Range(0, 2) * 2 - 1;

                        findedNumber += vector;

                        if (findedNumber + _needForActivate > _maxNumber)
                            findedNumber -= 2;

                        if (findedNumber < _minNumber)
                            findedNumber += 2;

                        return _currentAvailableNumbers.Where(e => e.Number == findedNumber).First();
                    }

                    return number;
                }
            }

            // Если 4 подряд не найдено, ищем любую неотмеченную
            foreach (ISelectNumber number in _shuffleAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                {
                    return _currentAvailableNumbers.Where(e => e.Number == _maxNumber - _needForActivate + 1).First();
                }

                return number;
            }

            // Если неотмеченных нет (чего быть не должно) берем рандомную чтобы не проваливаться в ошибку
            foreach (ISelectNumber number in _currentAvailableNumbers)
            {
                if (number.Number + _needForActivate - 1 > _maxNumber)
                    continue;

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