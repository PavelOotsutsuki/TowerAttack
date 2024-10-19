namespace GameFields.Persons
{
    public class StartPlayerTurnView: IPersonStep
    {
        private StartPlayerTurnLabel _label;

        public StartPlayerTurnView(StartPlayerTurnLabel label)
        {
            _label = label;
        }

        public bool IsComplete => _label.IsComplete;

        public void StartStep()
        {
            _label.Activate();
        }
    }
}
