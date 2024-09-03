using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using Zenject;

namespace GameFields.Persons
{
    public class EnemyAI : Person
    {
        private ITableDeactivator _tableDeactivator;

        public EnemyAI(ITableDeactivator tableDeactivator, ITurnStep enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus, Hand hand) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, enemyDragAndDropImitation,discoverImitation, bus, hand)
        {
            _tableDeactivator = tableDeactivator;
        }

        protected override void OnStartStep()
        {
            _tableDeactivator.Deactivate();
        }
    }
}
