using GameFields.StartFights;

namespace GameFields.Persons
{
    public class StartPlayerTurnView: ITurnStep
    {
        //private bool _isComplete;

        private StartPlayerTurnLabel _label;

        public StartPlayerTurnView(StartPlayerTurnLabel label)
        {
            //_isComplete = false;
            _label = label;
        }

        public bool IsComplete => _label.IsComplete;

        public void StartStep()
        {
            //_isComplete = false;

            _label.Activate();
        }
    }
}
