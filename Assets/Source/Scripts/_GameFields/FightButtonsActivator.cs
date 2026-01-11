using GameFields.FightMenues;
using GameFields.Histories;
using Tools;

namespace GameFields
{
    public class FightButtonsActivator : IWorkable
    {
        private readonly IWorkable _fightMenuActivateButton;
        private readonly IWorkable _historyMenuActivateButton;

        public FightButtonsActivator(FightMenuActivateButton fightMenuActivateButton, HistoryMenuActivateButton historyMenuActivateButton)
        {
            _fightMenuActivateButton = fightMenuActivateButton;
            _historyMenuActivateButton = historyMenuActivateButton;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _fightMenuActivateButton.Activate();
            _historyMenuActivateButton.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _fightMenuActivateButton.Deactivate();
            _historyMenuActivateButton.Deactivate();
        }
    }
}