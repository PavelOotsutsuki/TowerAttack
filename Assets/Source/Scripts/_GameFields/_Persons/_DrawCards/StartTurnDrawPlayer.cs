namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawPlayer : StartTurnDraw, IPlayerObject
    {
        public StartTurnDrawPlayer(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, countDrawCards)
        { }
    }
}