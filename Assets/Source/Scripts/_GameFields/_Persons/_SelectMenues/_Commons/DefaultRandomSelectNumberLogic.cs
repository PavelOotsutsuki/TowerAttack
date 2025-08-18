using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class DefaultRandomSelectNumberLogic : IRandomSelectNumberLogic
    {
        private readonly int _needForActivate;
        private readonly IEnumerable<ISelectNumber> _shuffleNumbers;
        private readonly ConfirmableNumbers _confirmableNumbers;

        public DefaultRandomSelectNumberLogic(int needForActivate, IEnumerable<ISelectNumber> currentAvailableNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _needForActivate = needForActivate;
            _confirmableNumbers = confirmableNumbers;

            _shuffleNumbers = Shuffle(currentAvailableNumbers);
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
            foreach (ISelectNumber number in _shuffleNumbers)
            {
                if (_confirmableNumbers.Contains(number.Number) == false && alreadySelectedNumbers.Contains(number) == false)
                {
                    return number;
                }
            }

            // Доходим до сюда если свободных номеров нет. Берем рандомный невыбранный
            foreach (ISelectNumber number in _shuffleNumbers)
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