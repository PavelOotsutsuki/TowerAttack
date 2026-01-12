using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SelectNumberActivateData : IData
    {
        private readonly NumberAnimationType? _numberAnimationType;
        private readonly SelectNumberClickHandler _selectNumberClickHandler;

        public SelectNumberActivateData(NumberAnimationType? numberAnimationType, SelectNumberClickHandler selectNumberClickHandler)
        {
            _numberAnimationType = numberAnimationType;
            _selectNumberClickHandler = selectNumberClickHandler;
        }

        public NumberAnimationType? NumberAnimationType => _numberAnimationType;
        public SelectNumberClickHandler SelectNumberClickHandler => _selectNumberClickHandler;
        public SelectNumberAnimatorActivateData SelectNumberAnimatorActivateData => new SelectNumberAnimatorActivateData(_numberAnimationType);
    }
}