namespace Tools.UI
{
    public class LabelActivateData : IData
    {
        private readonly string _message;

        public LabelActivateData(string message)
        {
            _message = message;
        }

        public string Message => _message;
    }
}