using Tools;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuEnemyAIActivateData : IData
    {
        private readonly string _message;

        public LookCardMenuEnemyAIActivateData(string message)
        {
            _message = message;
        }

        public string Message => _message;
    }
}