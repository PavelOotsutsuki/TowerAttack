using Tools;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumberActivateData : IData
    {
        private readonly NumberAnimationType? _numberAnimationType;

        public SelectNumberActivateData(NumberAnimationType? numberAnimationType)
        {
            _numberAnimationType = numberAnimationType;
        }

        public NumberAnimationType? NumberAnimationType => _numberAnimationType;
        public SelectNumberAnimatorActivateData SelectNumberAnimatorActivateData => new SelectNumberAnimatorActivateData(_numberAnimationType);
    }
}