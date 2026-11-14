using System.Threading;

namespace GameFields.Persons.DrawCards
{
    internal class StartTurnDrawEnemyAI : StartTurnDraw, IEnemyAIObject
    {
        public StartTurnDrawEnemyAI(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            int countDrawCards, CancellationToken turnToken) : base(gameFieldObjectsActivator, drawCardRoot, countDrawCards, turnToken)
        { }
    }
}