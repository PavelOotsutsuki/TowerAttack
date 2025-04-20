using Tools;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackMenuActivateData : IData
    {
        private readonly int _needSelect;

        public AttackMenuActivateData(int needSelect)
        {
            _needSelect = needSelect;
        }

        public int NeedSelect => _needSelect;
    }
}