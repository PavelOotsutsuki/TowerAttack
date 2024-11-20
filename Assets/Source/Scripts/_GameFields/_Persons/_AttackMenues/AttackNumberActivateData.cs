using Tools;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberActivateData : IData
    {
        //private readonly Sprite _sprite;

        //public AttackNumberActivateData(Sprite sprite)
        //{
        //    _sprite = sprite;
        //}

        //public Sprite Sprite => _sprite;
        //private readonly bool _isActiveView;

        //public AttackNumberActivateData(bool isActiveView)
        //{
        //    _isActiveView = isActiveView;
        //}

        //public bool IsActiveView => _isActiveView;
        private readonly AttackNumberStateViewActivateData _stateViewData;

        public AttackNumberActivateData(AttackNumberStateViewActivateData stateViewData)
        {
            _stateViewData = stateViewData;
        }

        public AttackNumberStateViewActivateData StateViewData => _stateViewData;
    }
}