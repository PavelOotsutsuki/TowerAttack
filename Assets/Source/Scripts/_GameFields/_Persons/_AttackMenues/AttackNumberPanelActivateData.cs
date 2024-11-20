using System.Collections;
using System.Collections.Generic;
using Tools;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberPanelActivateData: IData
    {
        private readonly int _needForActivate;

        public AttackNumberPanelActivateData(int needForActivate)
        {
            _needForActivate = needForActivate;
        }

        public int NeedForActivate => _needForActivate;
    }
}