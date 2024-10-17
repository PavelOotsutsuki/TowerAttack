using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;
using GameFields.Persons.AttackMenues;

namespace GameFields.Persons
{
    public class Player : Person
    {
        private readonly ITableActivator _tableActivator;
        private readonly IBlockable _handBlockable;
        private readonly IPersonStep _startPlayerTurnView;

        private AttackMenu _attackMenu;

        public Player(ITableActivator tableActivator, Hand hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, IPersonStep turnProcessing,
            SignalBus bus, IPersonStep startPlayerTurnView, AttackMenu attackMenu) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, turnProcessing, discover, bus, hand, attackMenu)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _tableActivator = tableActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
        }

        protected override void OnStartStep()
        {
            EnqueueStep(_startPlayerTurnView);

            _handBlockable.ForciblyBlock();
            _tableActivator.Activate();
            _attackMenu.Activate();
        }
    }
}