using System.Threading;

namespace GameFields.Persons.DrawCards
{
    internal class StartTurnDrawPlayer : StartTurnDraw, IPlayerObject
    {
        public StartTurnDrawPlayer(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            int countDrawCards, CancellationToken turnToken) : base(gameFieldObjectsActivator, drawCardRoot, countDrawCards, turnToken)
        { }
    }
}