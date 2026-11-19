using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SelectNumberAnimatorActivateData : IData
    {
        private readonly NumberAnimationType? _numberAnimationType;

        public SelectNumberAnimatorActivateData(NumberAnimationType? numberAnimationType)
        {
            _numberAnimationType = numberAnimationType;
        }

        public NumberAnimationType? NumberAnimationType => _numberAnimationType;
    }
}