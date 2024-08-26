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

        public Player(ITableActivator tableActivator, IBlockable hand, CardPlayingZone cardPlayingZone, Tower tower,
            Discover discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, ITurnStep turnProcessing, SignalBus bus) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, turnProcessing, discover, bus)
        {
            _tableActivator = tableActivator;
            _handBlockable = hand;
        }

        protected override void OnStartStep()
        {
            _handBlockable.ForciblyBlock();
            _tableActivator.Activate();
        }
    }
}