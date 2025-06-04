using GameFields.Persons.Commons;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawEnemyAI : StartTurnDraw, IEnemyAIObject
    {
        public StartTurnDrawEnemyAI(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, countDrawCards)
        { }
    }
}