using System.Threading;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDrawEnemyAICreator
    {
        private readonly InteractionActivator _gameFieldObjectsActivator;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly int _countDrawCards;

        public StartTurnDrawEnemyAICreator(InteractionActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot, int countDrawCards)
        {
            _gameFieldObjectsActivator = gameFieldObjectsActivator;
            _drawCardRoot = drawCardRoot;
            _countDrawCards = countDrawCards;
        }

        internal StartTurnDrawEnemyAI Create(CancellationToken turnToken) => new StartTurnDrawEnemyAI(_gameFieldObjectsActivator, _drawCardRoot, _countDrawCards, turnToken);
    }
}