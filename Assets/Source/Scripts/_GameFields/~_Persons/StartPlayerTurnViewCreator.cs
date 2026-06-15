using System.Threading;

namespace GameFields.Persons
{
    public class StartPlayerTurnViewCreator
    {
        private readonly InteractionActivator _interactionActivator;
        private readonly StartPlayerTurnLabel _label;

        public StartPlayerTurnViewCreator(InteractionActivator interactionActivator, StartPlayerTurnLabel label)
        {
            _interactionActivator = interactionActivator;
            _label = label;
        }

        internal StartPlayerTurnView Create(CancellationToken turnToken) => new StartPlayerTurnView(_interactionActivator, _label, turnToken);
    }
}