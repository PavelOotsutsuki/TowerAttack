using System.Threading;

namespace GameFields.Persons
{
    internal class PlayerSkipTurnView : SkipTurnView
    {
        public PlayerSkipTurnView(InteractionActivator interactionActivator, SkipTurnLabel label, CancellationToken turnToken) : base(interactionActivator, label, turnToken)
        { }
    }
}