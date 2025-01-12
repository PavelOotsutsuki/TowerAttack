using Tools;

namespace GameFields.LightControls
{
    public class LightController : IWorkable
    {
        private readonly LightPanel _lightPanel;
        private readonly LightableObject[] _lightableObjects;

        public LightController(LightPanel lightPanel, LightableObject[] lightableObjects)
        {
            _lightPanel = lightPanel;
            _lightableObjects = lightableObjects;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
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
    }
}