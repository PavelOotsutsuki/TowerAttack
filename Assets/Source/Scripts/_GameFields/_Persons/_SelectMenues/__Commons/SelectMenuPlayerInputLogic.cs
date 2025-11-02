using GameFields.InputSettings;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectMenuPlayerInputLogic : IInputLogic, IEnterPressHandler, IQPressHandler
    {
        private readonly IEnterPressHandler _enterPressHandler;
        private readonly IQPressHandler _qPressHandler;

        public SelectMenuPlayerInputLogic(SelectMenuPlayer selectMenuPlayer)
        {
            _enterPressHandler = selectMenuPlayer;
            _qPressHandler = selectMenuPlayer;
        }

        public void OnEnter()
        {
            _enterPressHandler.OnEnter();
        }

        public void OnQ()
        {
            _qPressHandler.OnQ();
        }
    }
}