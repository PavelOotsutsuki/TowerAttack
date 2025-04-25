using GameFields.Persons.SelectMenues.Commons;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class SetChoiceResultData : SetSelectResultData
    {
        private readonly string _message;

        public SetChoiceResultData(ResultType resultType, string message) : base(resultType)
        {
            _message = message;
        }

        public string Message => _message;
    }
}