using Tools;

namespace GameFields.Persons.SelectMenues
{
    public abstract class SelectNumberClickHandler
    {
        protected readonly int NeedForActivate;
        private readonly IWorkable _selectButton;

        private int _activateCounter;

        public SelectNumberClickHandler(int needForActivate, IWorkable selectButton)
        {
            NeedForActivate = needForActivate;
            _selectButton = selectButton;

            ActivateCounter = 0;
        }

        protected int ActivateCounter
        {
            get
            {
                return _activateCounter;
            }
            set
            {
                _activateCounter = value;

                if (_activateCounter == NeedForActivate)
                {
                    _selectButton.Activate();
                }
                else
                {
                    _selectButton.Deactivate();
                }
            }
        }

        public abstract bool CanBeClicked(SelectNumber currentNumber);
        public abstract void OnEnterClick();
        public abstract void OnExitClick();

        public void Reset()
        {
            ActivateCounter = 0;
        }
    }
}