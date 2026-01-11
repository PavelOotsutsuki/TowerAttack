namespace GameFields.Persons.Commons
{
    public class StartPlayerTurnView: PersonStep
    {
        private readonly StartPlayerTurnLabel _label;

        public StartPlayerTurnView(InteractionActivator interactionActivator, StartPlayerTurnLabel label)
            :base(interactionActivator)
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