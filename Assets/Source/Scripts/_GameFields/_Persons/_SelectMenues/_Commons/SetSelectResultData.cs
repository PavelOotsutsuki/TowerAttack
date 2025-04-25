using Tools;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SetSelectResultData : IData
    {
        private readonly ResultType _resultType;

        public SetSelectResultData(ResultType resultType)
        {
            _resultType = resultType;
        }

        public ResultType ResultType => _resultType;
    }
}