using System.Threading;

namespace GameFields.Persons
{
    internal class StartPlayerTurnView: PersonStep
    {
        private readonly StartPlayerTurnLabel _label;

        public StartPlayerTurnView(InteractionActivator interactionActivator, StartPlayerTurnLabel label, CancellationToken turnToken)
            :base(interactionActivator, turnToken)
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