namespace GameFields.Persons.SelectMenues
{
    public class SelectResult
    {
        private SetSelectResultData _data;

        public SelectResult()
        {
            _data = null;
        }

        public SetSelectResultData Data => _data;

        public void SetResult(SetSelectResultData data)
        {
            if (_data != null)
                if (_data.ResultType == ResultType.Success)
                    return;

            _data = data;
        }
    }
}