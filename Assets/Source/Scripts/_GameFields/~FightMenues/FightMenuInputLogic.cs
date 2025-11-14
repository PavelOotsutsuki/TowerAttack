using GameFields.InputSettings;

namespace GameFields.FightMenues
{
    public class FightMenuInputLogic : IInputLogic, IEnterPressHandler, IDownArrowPressHandler, IUpArrowPressHandler
    {
        private readonly IFightMenuInputActivateWatcher _fightMenuInputActivateWatcher;

        public FightMenuInputLogic(IFightMenuInputActivateWatcher fightMenuInputActivateWatcher)
        {
            _fightMenuInputActivateWatcher = fightMenuInputActivateWatcher;
        }

        public void OnEnter()
        {
            _fightMenuInputActivateWatcher.CurrentFightMenuButtonInputHandler.OnEnterPress();
        }

        public void OnDownArrow()
        {
            _fightMenuInputActivateWatcher.CurrentFightMenuButtonInputHandler.OnDownArrow();
        }

        public void OnUpArrow()
        {
            _fightMenuInputActivateWatcher.CurrentFightMenuButtonInputHandler.OnUpArrow();
        }
    }
}