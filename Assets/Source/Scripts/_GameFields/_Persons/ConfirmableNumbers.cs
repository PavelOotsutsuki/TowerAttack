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

        public void AddAccept(IAttackNumber attackNumber)
        {
            if (_attackedNumbers.Contains(attackNumber) == false)
                _attackedNumbers.Add(attackNumber);
        }

        public void AddSelect(ISelectNumber choicedNumber)
        {
            if (_choicedNumbers.Contains(choicedNumber) == false)
                _choicedNumbers.Add(choicedNumber);
        }

        ////public void Remove(IAttackNumber attackNumber)
        ////{
        ////    if (_acceptNumbers.Contains(attackNumber) == false)
        ////        _acceptNumbers.Remove(attackNumber);
        ////}

        public bool ContainsAccept(IAttackNumber attackNumber)
        {
            return _attackedNumbers.Contains(attackNumber);
        }

        public bool ContainsSelect(ISelectNumber choicedNumber)
        {
            return _choicedNumbers.Contains(choicedNumber);
        }

        //public void Clear()
        //{
        //    _acceptNumbers.Clear();
        //    _choicedNumbers.Clear();
        //}
    }
}