using System.Threading;

namespace GameFields.Persons
{
    public class PlayerSkipTurnViewCreator
    {
        private readonly InteractionActivator _interactionActivator;
        private readonly SkipTurnLabel _label;

        public PlayerSkipTurnViewCreator(InteractionActivator interactionActivator, SkipTurnLabel label)
        {
            _interactionActivator = interactionActivator;
            _label = label;
        }

        internal PlayerSkipTurnView Create(CancellationToken turnToken) => new PlayerSkipTurnView(_interactionActivator, _label, turnToken);
    }
}