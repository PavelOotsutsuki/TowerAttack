using Cards;
using Tools;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightController: IWorkable<CardDragAndDropLightControllerActivateData>, IBlockable
    {
        private readonly LightController _defaultLightController;
        private readonly LightController _gnomeLightController;

        private readonly LightController[] _allControls;
        private LightController _currentLightController;

        public CardDragAndDropLightController(LightController defaultLightController, LightController gnomeLightController)
        {
            _defaultLightController = defaultLightController;
            _gnomeLightController = gnomeLightController;

            _allControls = new LightController[]
            {
                _defaultLightController,
                _gnomeLightController
            };
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate(CardDragAndDropLightControllerActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if ((data.EffectFeature & CardCapability.GnomeForging) == CardCapability.GnomeForging)
            {
                _currentLightController = _gnomeLightController;
            }
            else
            {
                _currentLightController = _defaultLightController;
            }

            _currentLightController.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _currentLightController?.Deactivate();
        }

        public void Block()
        {
            foreach (LightController lightController in _allControls)
                lightController.Block();
        }

        public void Unblock()
        {
            foreach (LightController lightController in _allControls)
                lightController.Unblock();
        }
    }
}