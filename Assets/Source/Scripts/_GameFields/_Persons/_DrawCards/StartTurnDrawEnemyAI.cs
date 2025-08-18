using GameFields.Persons.Commons;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawEnemyAI : StartTurnDraw, IEnemyAIObject
    {
        public StartTurnDrawEnemyAI(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, countDrawCards)
        { }
    }
}