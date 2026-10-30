using System;
using System.Collections.Generic;
using System.Linq;
using GameFields.Persons.SelectMenues;
using ModestTree;
using Servers;
using Tools.Settings;

namespace GameFields.Persons.ConfirmableNumbersView
{
    public class ConfirmableNumbers : INumbersStateWatcher
    {
        private readonly SelectNumbersList _attackedNumbers;
        private readonly SelectNumbersList _choicedNumbers;
        private readonly SelectNumbersList _cursedNumbers;

        //private readonly FightProcessDBManager _fightProcessDBManager;

        private readonly int[] _allNumbers;

        public ConfirmableNumbers(SelectNumbersList attackedNumbers, SelectNumbersList choicedNumbers,
            SelectNumbersList cursedNumbers)
        {
            _attackedNumbers = attackedNumbers;
            _choicedNumbers = choicedNumbers;
            _cursedNumbers = cursedNumbers;

            //_fightProcessDBManager = fightProcessDBManager;

            _attackedNumbers.OnChanged += ActionOnChanged;
            _choicedNumbers.OnChanged += ActionOnChanged;
            _cursedNumbers.OnChanged += ActionOnChanged;

            _allNumbers = GameSettings.DefaultCardNumbers;
        }

        ~ConfirmableNumbers()
        {
            _attackedNumbers.OnChanged -= ActionOnChanged;
            _choicedNumbers.OnChanged -= ActionOnChanged;
            _cursedNumbers.OnChanged -= ActionOnChanged;
        }

        public event Action OnChanged;

        public IEnumerable<int> FreeNumbers => _allNumbers.Except(FullList.Select(p => p.Key));
        public IEnumerable<int> CheckedNumbers => FullList.Select(p => p.Key);

        public IReadOnlyDictionary<int, NumberAnimationType> FullList
        {
            get
            {
                //SelectNumbersList fullList = new SelectNumbersList(null, null);

                //AddRange(fullList, _attackedNumbers.SelectedNumbersStates);
                //AddRange(fullList, _cursedNumbers.SelectedNumbersStates);
                //AddRange(fullList, _choicedNumbers.SelectedNumbersStates);

                //return fullList;

                Dictionary<int, NumberAnimationType> fullList = new Dictionary<int, NumberAnimationType>();

                AddRange(fullList, _attackedNumbers.SelectedNumbersStates, _attackedNumbers.GetNumberAnimationType());
                AddRange(fullList, _cursedNumbers.SelectedNumbersStates, _cursedNumbers.GetNumberAnimationType());
                AddRange(fullList, _choicedNumbers.SelectedNumbersStates, _choicedNumbers.GetNumberAnimationType());

                return fullList;
            }
        }

        public int Count => FullList.Count;

        public void Clear()
        {
            _attackedNumbers.Clear();
            _choicedNumbers.Clear();
            _cursedNumbers.Clear();
        }

        private void ActionOnChanged()
        {
            OnChanged?.Invoke();
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

        //public void AddAccept(ISelectNumber attackNumber)
        //{
        //    if (_attackedNumbers.Contains(attackNumber) == false)
        //        _attackedNumbers.Add(attackNumber);
        //}

        //public void AddSelect(ISelectNumber choicedNumber)
        //{
        //    if (_choicedNumbers.Contains(choicedNumber) == false)
        //        _choicedNumbers.Add(choicedNumber);
        //}

        ////public void Remove(IAttackNumber attackNumber)
        ////{
        ////    if (_acceptNumbers.Contains(attackNumber) == false)
        ////        _acceptNumbers.Remove(attackNumber);
        ////}

        //public bool ContainsAccept(int attackNumber)
        //{
        //    return _attackedNumbers.Contains(attackNumber);
        //}

        //public bool ContainsSelect(int choicedNumber)
        //{
        //    return _choicedNumbers.Contains(choicedNumber);
        //}

        public bool Contains(int selectedNumber)
        {
            return _choicedNumbers.Contains(selectedNumber) || _attackedNumbers.Contains(selectedNumber) || _cursedNumbers.Contains(selectedNumber);
        }

        private void AddRange(Dictionary<int, NumberAnimationType> addedList, IReadOnlyList<int> clonedList, NumberAnimationType type)
        {
            foreach (int number in clonedList)
            {
                if (addedList.ContainsKey(number) == false)
                {
                    addedList.Add(number, type);
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