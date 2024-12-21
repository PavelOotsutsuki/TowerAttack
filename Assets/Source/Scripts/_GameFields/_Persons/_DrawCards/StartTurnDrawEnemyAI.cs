namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawEnemyAI : StartTurnDraw
    {
        public StartTurnDrawEnemyAI(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, countDrawCards)
        { }
    }
}