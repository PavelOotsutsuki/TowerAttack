using Tools;

namespace GameFields.LightControls
{
    public class LightController : IWorkable, IBlockable
    {
        private readonly LightPanel _lightPanel;
        private readonly LightableObject[] _lightableObjects;
        private bool _isActivatable;

        public LightController(LightPanel lightPanel, LightableObject[] lightableObjects)
        {
            _lightPanel = lightPanel;
            _lightableObjects = lightableObjects;

            _isActivatable = true;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (_isActivatable == false)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

            foreach (LightableObject lightableObject in _lightableObjects)
            {
                lightableObject.Show();
            }

            _lightPanel.Show();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            foreach (LightableObject lightableObject in _lightableObjects)
            {
                lightableObject.Hide();
            }

            _lightPanel.Hide();
        }

        public void Unblock()
        {
            _isActivatable = true;
        }

        public void Block()
        {
            _isActivatable = false;

            Deactivate();
        }
    }
}