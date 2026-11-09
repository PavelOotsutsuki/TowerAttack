using Tools.InputSettings;

namespace Menues
{
    public class MenuInputLogic : IInputLogic, IEnterPressHandler, IDownArrowPressHandler, IUpArrowPressHandler, ILeftArrowPressHandler, IRightArrowPressHandler
    {
        private readonly IMenuInputActivateWatcher _menuInputActivateWatcher;

        public MenuInputLogic(IMenuInputActivateWatcher fightMenuInputActivateWatcher)
        {
            _menuInputActivateWatcher = fightMenuInputActivateWatcher;
        }

        public void OnEnter()
        {
            _menuInputActivateWatcher.CurrentMenuButtonInputHandler.OnEnterPress();
        }

        public void OnDownArrow()
        {
            _menuInputActivateWatcher.CurrentMenuButtonInputHandler.OnDownArrow();
        }

        public void OnUpArrow()
        {
            _menuInputActivateWatcher.CurrentMenuButtonInputHandler.OnUpArrow();
        }

        public void OnLeftArrow()
        {
            _menuInputActivateWatcher.CurrentMenuButtonInputHandler.OnLeftArrow();
        }

        public void OnRightArrow()
        {
            _menuInputActivateWatcher.CurrentMenuButtonInputHandler.OnRightArrow();
        }
    }
}