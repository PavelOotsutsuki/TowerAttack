using System;
using System.Collections.Generic;
using Servers;
using System.Text.Json;

namespace GameFields.Persons.SelectMenues
{
    public abstract class SelectNumbersList
    {
        private readonly List<int> _selectedNumbersStates;
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly bool? _isPlayersObject;
        private readonly SelectNumbersListType _type;
        private readonly NumberAnimationType _numberAnimationType;

        internal event Action OnChanged;

        // Не нравится конечно что при FullList в ConfirmableNumbers создается экземпляр с null-ями тут
        public SelectNumbersList(FightProcessDBManager fightProcessDBManager, bool? isPlayersObject)
        {
            _selectedNumbersStates = new List<int>();

            _fightProcessDBManager = fightProcessDBManager;
            _isPlayersObject = isPlayersObject;

            _type = SetSelectNumbersListType();
            _numberAnimationType = ConvertSelectNumbersListTypeToNumberAnimationType();
        }

        public IReadOnlyList<int> SelectedNumbersStates => _selectedNumbersStates;
        public SelectNumbersListType Type => _type;

        public void Add(int selectNumber)//, NumberAnimationType type)
        {
            //if (IsValidAnimationType(type) == false)
            //    throw new Exception($"Попытка передать невалидный NumberAnimationType ({type}) в SelectNumbersList ({_type}). Owner player?: {_isPlayersObject}");
            //NumberAnimationType type = _numberAnimationType;

            if (_selectedNumbersStates.Contains(selectNumber) == false)
            {
                _selectedNumbersStates.Add(selectNumber);
                _fightProcessDBManager?.WriteFightProcessAction(Fight.TurnNumber, _isPlayersObject, selectNumber.ToString(), "SELECTED", _type.ToString());
                _fightProcessDBManager?.WriteFightProcessAction(Fight.TurnNumber, _isPlayersObject, GetSerializedSelectedNumbersStates(), "FULLLIST", GetType().Name);
                OnChanged?.Invoke();
            }
        }

        //public void Add(int selectNumber)
        //{
        //    if (_selectedNumbersStates.ContainsKey(selectNumber) == false)
        //    {
        //        _selectedNumbersStates.Add(selectNumber, _numberAnimationType);
        //        _fightProcessDBManager?.WriteFightProcessAction(Fight.TurnNumber, _isPlayersObject, selectNumber.ToString(), "SELECTED", _numberAnimationType.ToString());
        //        OnChanged?.Invoke();
        //    }
        //}

        public bool Contains(int selectNumber)
        {
            return _selectedNumbersStates.Contains(selectNumber);
        }

        public void Clear()
        {
            if (_selectedNumbersStates.Count == 0)
                return;

            _selectedNumbersStates.Clear();
            _fightProcessDBManager?.WriteFightProcessAction(Fight.TurnNumber, _isPlayersObject, null, "CLEAR", _type.ToString());
            _fightProcessDBManager?.WriteFightProcessAction(Fight.TurnNumber, _isPlayersObject, GetSerializedSelectedNumbersStates(), "FULLLIST", GetType().Name);
            OnChanged?.Invoke();
        }

        public NumberAnimationType GetNumberAnimationType()
        {
            return _numberAnimationType;
        }

        //private bool IsValidAnimationType(NumberAnimationType type)
        //{
        //    switch (_type)
        //    {
        //        case SelectNumbersListType.Attack:
        //            return type == NumberAnimationType.Error || type == NumberAnimationType.Success;
        //        case SelectNumbersListType.Choice:
        //            return type == NumberAnimationType.Choice;
        //        case SelectNumbersListType.Curse:
        //            return type == NumberAnimationType.Curse;
        //        default:
        //            throw new Exception("Unknown SelectNumbersListType: " + _type.ToString());
        //    }
        //}

        private NumberAnimationType ConvertSelectNumbersListTypeToNumberAnimationType()
        {
            switch (_type)
            {
                case SelectNumbersListType.Attack:
                    return NumberAnimationType.Error;
                case SelectNumbersListType.Choice:
                    return NumberAnimationType.Choice;
                case SelectNumbersListType.Curse:
                    return NumberAnimationType.Curse;
                default:
                    throw new Exception("Unknown SelectNumbersListType: " + _type.ToString());
            }
        }

        private string GetSerializedSelectedNumbersStates()
        {
            return JsonSerializer.Serialize(_selectedNumbersStates);
        }

        protected abstract SelectNumbersListType SetSelectNumbersListType();
    }
}