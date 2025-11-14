namespace Tools.UI
{
    public class PointerDisableSettings
    {
        private bool _isDisable;

        public PointerDisableSettings()
        {
            _isDisable = false;
        }

        public bool IsDisable => _isDisable;

        public void Disable()
        {
            _isDisable = true;
        }

        public void Enable()
        {
            _isDisable = false;
        }
    }
}