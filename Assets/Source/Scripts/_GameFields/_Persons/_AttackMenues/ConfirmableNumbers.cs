using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class ConfirmableNumbers
    {
        private readonly List<IAttackNumber> _acceptNumbers;

        public ConfirmableNumbers()
        {
            _acceptNumbers = new List<IAttackNumber>();
        }

        public IReadOnlyList<IAttackNumber> AcceptNumbers => _acceptNumbers;

        public void Add(IAttackNumber attackNumber)
        {
            if (_acceptNumbers.Contains(attackNumber) == false)
                _acceptNumbers.Add(attackNumber);
        }

        public void Remove(IAttackNumber attackNumber)
        {
            if (_acceptNumbers.Contains(attackNumber) == false)
                _acceptNumbers.Remove(attackNumber);
        }

        public bool Contains(IAttackNumber attackNumber)
        {
            return _acceptNumbers.Contains(attackNumber);
        }

        public void Clear()
        {
            _acceptNumbers.Clear();
        }
    }
}