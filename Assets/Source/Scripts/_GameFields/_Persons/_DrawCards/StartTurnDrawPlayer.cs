using GameFields.Persons.Commons;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawPlayer : StartTurnDraw, IPlayerObject
    {
        public StartTurnDrawPlayer(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            int countDrawCards) : base(gameFieldObjectsActivator, drawCardRoot, countDrawCards)
        { }
    }
}