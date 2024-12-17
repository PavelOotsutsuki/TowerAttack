namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawPlayer : StartTurnDraw
    {
        public StartTurnDrawPlayer(GameFieldObjectsActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, countDrawCards)
        { }
    }
}