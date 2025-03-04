using Tools;

namespace GameFields.Persons.AttackMenues
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