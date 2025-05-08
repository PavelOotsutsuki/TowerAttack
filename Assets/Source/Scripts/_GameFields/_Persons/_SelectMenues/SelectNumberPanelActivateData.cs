using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SelectNumberPanelActivateData : IData
    {
        private readonly int _needForActivate;
        private readonly SelectResult _selectResult;

        public SelectNumberPanelActivateData(int needForActivate, SelectResult selectResult)
        {
            _needForActivate = needForActivate;
            _selectResult = selectResult;
        }

        public int NeedForActivate => _needForActivate;
        public SelectResult SelectResult => _selectResult;
    }
}