using GameFields.DiscardPiles;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;

namespace GameFields.Persons
{
    public class EnemyAI : Person
    {
        private DiscoverImitation _discoverImitation;
        private ITableDeactivator _tableDeactivator;

        public EnemyAI(DiscardPile discardPile, ITableDeactivator tableDeactivator, EnemyDragAndDropImitation enemyDragAndDropImitation, HandAI hand, Table table, Tower tower,
            DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation, StartTurnDraw startTurnDraw) :
            base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw, enemyDragAndDropImitation)
        {
            _discoverImitation = discoverImitation;
            _tableDeactivator = tableDeactivator;
        }

        protected override void OnStartStep()
        {
            _tableDeactivator.Deactivate();
        }
    }
}
