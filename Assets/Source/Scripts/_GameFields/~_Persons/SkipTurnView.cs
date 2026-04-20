namespace GameFields.Persons
{
    public abstract class SkipTurnView : PersonStep
    {
        private readonly SkipTurnLabel _label;

        public SkipTurnView(InteractionActivator interactionActivator, SkipTurnLabel label)
            : base(interactionActivator)
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