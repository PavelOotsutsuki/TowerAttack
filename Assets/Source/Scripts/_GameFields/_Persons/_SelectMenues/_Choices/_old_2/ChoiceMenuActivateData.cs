using GameFields.Persons.SelectMenues.Commons;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceMenuActivateData : SelectMenuActivateData
    {
        private readonly ISelectResultHandler _selectResultHandler;

        public ChoiceMenuActivateData(int needSelect, ISelectResultHandler selectResultHandler) : base(needSelect)
        {
            _selectResultHandler = selectResultHandler;
        }

        public ISelectResultHandler SelectResultHandler => _selectResultHandler;
    }
}