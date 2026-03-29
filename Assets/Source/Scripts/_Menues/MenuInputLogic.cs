using Tools.InputSettings;

namespace Menues
{
    public class MenuInputLogic : IInputLogic, IEnterPressHandler, IDownArrowPressHandler, IUpArrowPressHandler
    {
        private readonly IMenuInputActivateWatcher _fightMenuInputActivateWatcher;

        public MenuInputLogic(IMenuInputActivateWatcher fightMenuInputActivateWatcher)
        {
            _fightMenuInputActivateWatcher = fightMenuInputActivateWatcher;
        }

        public void OnEnter()
        {
            _fightMenuInputActivateWatcher.CurrentMenuButtonInputHandler.OnEnterPress();
        }

        public void OnDownArrow()
        {
            _fightMenuInputActivateWatcher.CurrentMenuButtonInputHandler.OnDownArrow();
        }

        public void OnUpArrow()
        {
            _fightMenuInputActivateWatcher.CurrentMenuButtonInputHandler.OnUpArrow();
        }
    }
}