using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SetSelectResultData : IData
    {
        private readonly ResultType _resultType;
        private readonly string _message;

        public SetSelectResultData(ResultType resultType, string message)
        {
            _resultType = resultType;
            _message = message;
        }

        public SetSelectResultData(ResultType resultType) : this(resultType, null)
        { }

        public ResultType ResultType => _resultType;
        public string Message => _message ?? throw new System.Exception("Попытка получить Message, где не надо");
    }
}