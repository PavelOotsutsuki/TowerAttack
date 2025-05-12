using Tools;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumberPanelActivateData : IData
    {
        private readonly int _needForActivate;
        private readonly RestrictionType? _restrictionType;
        private readonly SelectResult _selectResult;

        public SelectNumberPanelActivateData(int needForActivate, RestrictionType? restrictionType, SelectResult selectResult)
        {
            _needForActivate = needForActivate;
            _restrictionType = restrictionType;
            _selectResult = selectResult;
        }

        public int NeedForActivate => _needForActivate;
        public RestrictionType? RestrictionType => _restrictionType;
        public SelectResult SelectResult => _selectResult;
    }
}