using System.Threading;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawPlayerCreator
    {
        private readonly InteractionActivator _gameFieldObjectsActivator;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly int _countDrawCards;

        public StartTurnDrawPlayerCreator(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot, int countDrawCards)
        {
            _gameFieldObjectsActivator = gameFieldObjectsActivator;
            _drawCardRoot = drawCardRoot;
            _countDrawCards = countDrawCards;
        }

        internal StartTurnDrawPlayer Create(CancellationToken turnToken) => new StartTurnDrawPlayer(_gameFieldObjectsActivator, _drawCardRoot, _countDrawCards, turnToken);
    }
}