namespace Tools.UI
{
    public class FadableLabelActivateData : IData
    {
        private readonly string _message;

        public FadableLabelActivateData(string message)
        {
            _message = message;
        }

        public string Message => _message;
    }
}