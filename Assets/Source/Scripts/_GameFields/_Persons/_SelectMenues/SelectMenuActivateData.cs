using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SelectMenuActivateData : IData
    {
        private readonly int _needSelect;

        public SelectMenuActivateData(int needSelect)
        {
            _needSelect = needSelect;
        }

        public int NeedSelect => _needSelect;
    }
}