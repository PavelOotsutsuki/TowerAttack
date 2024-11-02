using Tools;

namespace GameFields.Persons.Discovers
{
    public class DiscoverLabelActivateData: IData
    {
        private readonly string _message;

        public DiscoverLabelActivateData(string message)
        {
            _message = message;
        }

        public string Message => _message;
    }
}