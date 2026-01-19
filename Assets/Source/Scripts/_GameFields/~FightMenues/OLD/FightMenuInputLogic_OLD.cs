using GameFields.InputSettings;
using Tools.InputSettings;

namespace GameFields.FightMenues
{
    public class FightMenuInputLogic_OLD : IInputLogic, IEnterPressHandler, IDownArrowPressHandler, IUpArrowPressHandler
    {
        private readonly IFightMenuInputActivateWatcher_OLD _fightMenuInputActivateWatcher;

        public FightMenuInputLogic_OLD(IFightMenuInputActivateWatcher_OLD fightMenuInputActivateWatcher)
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