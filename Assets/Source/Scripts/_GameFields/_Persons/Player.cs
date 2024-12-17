using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;
using GameFields.Persons.AttackMenues;
using Tools;

namespace GameFields.Persons
{
    public class Player : Person
    {
        //private readonly IActivatable _gameFieldObjectsActivator;
        private readonly IHandBlockable _handBlockable;
        private readonly PersonStep _startPlayerTurnView;
        private readonly EndTurnProcessing _endTurnProcessing;

        private AttackMenu _attackMenu;

        public Player(GameFieldObjectsActivator gameFieldObjectsActivator, Hand hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, PersonStep turnProcessing,
            SignalBus bus, PersonStep startPlayerTurnView, AttackMenu attackMenu, EndTurnProcessing endTurnProcessing) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, turnProcessing, discover, bus, hand,
                attackMenu, gameFieldObjectsActivator)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _endTurnProcessing = endTurnProcessing;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
        }

        protected override void InitSteps()
        {
            EnqueueStep(_startPlayerTurnView);
            EnqueueStep(StartTurnDraw);
            EnqueueStep(TurnProcess);
            EnqueueStep(CardEffectProcessing);
            EnqueueStep(_endTurnProcessing);
        }

        protected override void OnStartStep()
        {
            //_handBlockable.ForciblyBlock();
            //GameFieldObjectsActivator.Activate();
            //_attackMenu.Activate();
        }
    }
}