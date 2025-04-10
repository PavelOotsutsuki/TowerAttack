using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.SelectMenues;
using UnityEngine;

namespace GameFields.Persons
{
    public class ConfirmableNumbers
    {
        private readonly List<IAttackNumber> _acceptNumbers;
        private readonly List<ISelectNumber> _selectedNumbers;

        public ConfirmableNumbers()
        {
            _acceptNumbers = new List<IAttackNumber>();
            _selectedNumbers = new List<ISelectNumber>();
        }

        public IReadOnlyList<IAttackNumber> AcceptNumbers => _acceptNumbers;
        public IReadOnlyList<ISelectNumber> SelectedNumbers => _selectedNumbers;

        public void AddAccept(IAttackNumber attackNumber)
        {
            if (_acceptNumbers.Contains(attackNumber) == false)
                _acceptNumbers.Add(attackNumber);
        }

        public void AddSelect(ISelectNumber selectNumber)
        {
            if (_selectedNumbers.Contains(selectNumber) == false)
                _selectedNumbers.Add(selectNumber);
        }

        //public void Remove(IAttackNumber attackNumber)
        //{
        //    if (_acceptNumbers.Contains(attackNumber) == false)
        //        _acceptNumbers.Remove(attackNumber);
        //}

        public bool ContainsAccept(IAttackNumber attackNumber)
        {
            return _acceptNumbers.Contains(attackNumber);
        }

        public bool ContainsSelect(ISelectNumber selectNumber)
        {
            return _selectedNumbers.Contains(selectNumber);
        }

        public void Clear()
        {
            _acceptNumbers.Clear();
            _selectedNumbers.Clear();
        }
    }
}