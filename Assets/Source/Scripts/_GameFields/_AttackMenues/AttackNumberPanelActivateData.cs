using Tools;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberPanelActivateData: IData
    {
        private readonly int _needToActivate;

        public AttackNumberPanelActivateData(int needToActivate)
        {
            _needToActivate = needToActivate;
        }

        public int NeedToActivate => _needToActivate;
    }
}