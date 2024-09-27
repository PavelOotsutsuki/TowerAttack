using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;

namespace GameFields.Persons
{
    public class Player : Person
    {
        private readonly ITableActivator _tableActivator;
        private readonly IBlockable _handBlockable;
        private readonly ITurnStep _startPlayerTurnView;

        public Player(ITableActivator tableActivator, Hand hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, ITurnStep turnProcessing, SignalBus bus, ITurnStep startPlayerTurnView) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, turnProcessing, discover, bus, hand)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _tableActivator = tableActivator;
            _handBlockable = hand;
        }

        protected override void OnStartStep()
        {
            EnqueueStep(_startPlayerTurnView);

            _handBlockable.ForciblyBlock();
            _tableActivator.Activate();
        }
    }
}