using System.Threading;
using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class SetSelectResultData : CancellationTokenData
    {
        private readonly ResultType _resultType;
        //private readonly IEnumerable<ISelectNumber> _currentSelectedNumbers;
        private readonly string _message;

        public SetSelectResultData(ResultType resultType, string message, CancellationToken token) : base(token)
        {
            _resultType = resultType;
            _message = message;
        }

        public SetSelectResultData(ResultType resultType, CancellationToken token) : this(resultType, null, token)
        { }

        public ResultType ResultType => _resultType;
        public string Message => _message ?? throw new System.Exception("Попытка получить Message, где не надо");
    }
}