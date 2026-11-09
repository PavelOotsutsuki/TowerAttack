using System.Threading;

namespace GameFields.Persons
{
    internal class EnemySkipTurnView : SkipTurnView
    {
        public EnemySkipTurnView(InteractionActivator interactionActivator, SkipTurnLabel label, CancellationToken turnToken) : base(interactionActivator, label, turnToken)
        { }
    }
}