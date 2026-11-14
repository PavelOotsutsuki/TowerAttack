using System.Threading;

namespace GameFields.Persons
{
    public class EnemySkipTurnViewCreator
    {
        private readonly InteractionActivator _interactionActivator;
        private readonly SkipTurnLabel _label;

        public EnemySkipTurnViewCreator(InteractionActivator interactionActivator, SkipTurnLabel label)
        {
            _interactionActivator = interactionActivator;
            _label = label;
        }

        internal EnemySkipTurnView Create(CancellationToken turnToken) => new EnemySkipTurnView(_interactionActivator, _label, turnToken);
    }
}
