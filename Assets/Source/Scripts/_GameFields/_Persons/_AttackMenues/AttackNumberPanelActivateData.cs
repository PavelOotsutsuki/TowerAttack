using Tools;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberPanelActivateData: IData
    {
        private readonly int _needForActivate;
        private readonly AttackResult _attackResult;

        public AttackNumberPanelActivateData(int needForActivate, AttackResult attackResult)
        {
            _needForActivate = needForActivate;
            _attackResult = attackResult;
        }

        public int NeedForActivate => _needForActivate;
        public AttackResult AttackResult => _attackResult;
    }
}