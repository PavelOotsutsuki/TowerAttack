using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class ConfirmableNumbers
    {
        private readonly List<AttackNumber> _acceptNumbers;

        public ConfirmableNumbers()
        {
            _acceptNumbers = new List<AttackNumber>();
        }

        public IReadOnlyList<AttackNumber> AcceptNumbers => _acceptNumbers;

        public void Add(AttackNumber attackNumber)
        {
            if (_acceptNumbers.Contains(attackNumber) == false)
                _acceptNumbers.Add(attackNumber);
        }

        public void Remove(AttackNumber attackNumber)
        {
            if (_acceptNumbers.Contains(attackNumber) == false)
                _acceptNumbers.Remove(attackNumber);
        }

        public bool Contains(AttackNumber attackNumber)
        {
            return _acceptNumbers.Contains(attackNumber);
        }

        public void Clear()
        {
            _acceptNumbers.Clear();
        }
    }
}