using Tools;
using Tools.Settings;

namespace GameFields.Persons.SelectMenues
{
    public class SelectMenuActivateData : IData
    {
        private readonly int _needSelect;
        private readonly RestrictionType? _restrictionType;

        public SelectMenuActivateData(int needSelect, RestrictionType? restrictionType = null)
        {
            if (GameSettings.DefaultCardNumbers.Length < needSelect)
            {
                _needSelect = GameSettings.DefaultCardNumbers.Length;
            }
            else
            {
                _needSelect = needSelect;
            }

            _restrictionType = restrictionType;
        }

        public int NeedSelect => _needSelect;
        public RestrictionType? RestrictionType => _restrictionType;
    }
}