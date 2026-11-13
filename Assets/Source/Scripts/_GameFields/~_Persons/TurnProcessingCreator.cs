using System.Threading;

namespace GameFields.Persons
{
    public class TurnProcessingCreator
    {
        private readonly InteractionActivator _interactionActivator;
        private readonly SkipTurnChecker _skipTurnChecker;

        public TurnProcessingCreator(InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker)
        {
            _interactionActivator = interactionActivator;
            _skipTurnChecker = skipTurnChecker;
        }

        internal TurnProcessing Create(CancellationToken turnToken) => new TurnProcessing(_interactionActivator, _skipTurnChecker, turnToken);
    }
}