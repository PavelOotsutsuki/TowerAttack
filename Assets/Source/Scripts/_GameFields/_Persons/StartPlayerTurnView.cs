namespace GameFields.Persons
{
    public class StartPlayerTurnView: PersonStep
    {
        private readonly StartPlayerTurnLabel _label;

        public StartPlayerTurnView(GameFieldObjectsActivator gameFieldObjectsActivator, StartPlayerTurnLabel label)
            :base(gameFieldObjectsActivator)
        {
            _label = label;
        }

        public override bool IsComplete => _label.IsComplete;

        protected override void OnStartStep()
        {
            _label.Activate();
        }
    }
}