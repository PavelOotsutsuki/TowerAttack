using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Persons
{
    public class ConfirmableNumbers
    {
        private readonly SelectNumbersList _attackedNumbers;
        private readonly SelectNumbersList _choicedNumbers;

        public ConfirmableNumbers(SelectNumbersList attackedNumbers,
            SelectNumbersList choicedNumbers)
        {
            _attackedNumbers = attackedNumbers;
            _choicedNumbers = choicedNumbers;
        }

        public SelectNumbersList FullList
        {
            get
            {
                SelectNumbersList fullList = new SelectNumbersList();

                AddRange(fullList, _attackedNumbers.SelectedNumbersStates);
                AddRange(fullList, _choicedNumbers.SelectedNumbersStates);

                return fullList;
            }
        }

        public int Count => FullList.SelectedNumbersStates.Count;

        public void Clear()
        {
            _attackedNumbers.Clear();
            _choicedNumbers.Clear();
        }

        //private readonly List<IAttackNumber> _acceptNumbers;
        //private readonly List<IChoiceNumber> _choicedNumbers;

        //public ConfirmableNumbers()
        //{
        //    _acceptNumbers = new List<IAttackNumber>();
        //    _choicedNumbers = new List<IChoiceNumber>();
        //}

        //public IReadOnlyList<IAttackNumber> AcceptNumbers => _acceptNumbers;
        //public IReadOnlyList<IChoiceNumber> ChoicedNumbers => _choicedNumbers;

        public void AddAccept(ISelectNumber attackNumber)
        {
            //if (_attackedNumbers.Contains(attackNumber) == false)
            //    _attackedNumbers.Add(attackNumber);
        }

        public void AddSelect(ISelectNumber choicedNumber)
        {
            //if (_choicedNumbers.Contains(choicedNumber) == false)
            //    _choicedNumbers.Add(choicedNumber);
        }

        ////public void Remove(IAttackNumber attackNumber)
        ////{
        ////    if (_acceptNumbers.Contains(attackNumber) == false)
        ////        _acceptNumbers.Remove(attackNumber);
        ////}

        public bool ContainsAccept(int attackNumber)
        {
            return _attackedNumbers.Contains(attackNumber);
        }

        public bool ContainsSelect(int choicedNumber)
        {
            return _choicedNumbers.Contains(choicedNumber);
        }

        public bool Contains(int selectedNumber)
        {
            return _choicedNumbers.Contains(selectedNumber) || _attackedNumbers.Contains(selectedNumber);
        }

        private void AddRange(SelectNumbersList addedList, IReadOnlyDictionary<int, NumberAnimationType> clonedList)
        {
            foreach (KeyValuePair<int, NumberAnimationType> number in clonedList)
            {
                if (addedList.Contains(number.Key) == false)
                {
                    addedList.Add(number.Key, number.Value);
                }
            }
        }
        //public void Clear()
        //{
        //    _acceptNumbers.Clear();
        //    _choicedNumbers.Clear();
        //}
    }
}