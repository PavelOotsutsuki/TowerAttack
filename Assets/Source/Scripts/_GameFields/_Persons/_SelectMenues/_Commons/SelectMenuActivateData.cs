using Tools;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectMenuActivateData : IData
    {
        private readonly int _needSelect;
        private readonly RestrictionType? _restrictionType;

        public SelectMenuActivateData(int needSelect, RestrictionType? restrictionType = null)
        {
            _needSelect = needSelect;
            _restrictionType = restrictionType;
        }

        public int NeedSelect => _needSelect;
        public RestrictionType? RestrictionType => _restrictionType;
    }
}