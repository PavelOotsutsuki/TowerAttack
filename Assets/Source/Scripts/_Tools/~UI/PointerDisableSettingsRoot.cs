namespace Tools.UI
{
    public class PointerDisableSettingsRoot
    {
        private readonly PointerDisableSettings _onPointerExit;

        public PointerDisableSettingsRoot()
        {
            _onPointerExit = new PointerDisableSettings();
        }

        public PointerDisableSettings OnPointerExit => _onPointerExit;
    }
}