using GameFields.InputSettings;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuPlayerInputLogic : IInputLogic, IEnterPressHandler, IRightArrowPressHandler, ILeftArrowPressHandler
    {
        private readonly IEnterPressHandler _enterPressHandler;
        private readonly IRightArrowPressHandler _rightArrowPressHandler;
        private readonly ILeftArrowPressHandler _leftArrowPressHandler;

        public LookCardMenuPlayerInputLogic(LookCardMenuPlayer lookCardMenuPlayer)
        {
            _enterPressHandler = lookCardMenuPlayer;
            _rightArrowPressHandler = lookCardMenuPlayer;
            _leftArrowPressHandler = lookCardMenuPlayer;
        }

        public void OnEnter()
        {
            _enterPressHandler.OnEnter();
        }

        public void OnRightArrow()
        {
            _rightArrowPressHandler.OnRightArrow();
        }

        public void OnLeftArrow()
        {
            _leftArrowPressHandler.OnLeftArrow();
        }
    }
}