using Tools.InputSettings;

namespace Menues
{
    public interface IMenuInputActivateWatcher
    {
        public IFocusedButtonEnterHandler CurrentMenuButtonInputHandler { get; }
    }
}