using System.Threading;

namespace GameFields.Persons
{
    internal abstract class SkipTurnView : PersonStep
    {
        private readonly SkipTurnLabel _label;

        public SkipTurnView(InteractionActivator interactionActivator, SkipTurnLabel label, CancellationToken turnToken)
            : base(interactionActivator, turnToken)
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