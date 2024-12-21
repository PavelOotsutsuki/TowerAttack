using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;
using GameFields.Persons.AttackMenues;
using Tools;
using Cards;

namespace GameFields.Persons
{
    public class Player : Person
    {
        //private readonly IActivatable _gameFieldObjectsActivator;
        private readonly IHandBlockable _handBlockable;
        private readonly PersonStep _startPlayerTurnView;
        private readonly EndTurnProcessing _endTurnProcessing;

        private readonly TurnProcessing _turnProcessing;

        private AttackMenu _attackMenu;

        public Player(InteractionActivator interactionActivator, Hand hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, TurnProcessing turnProcessing,
            SignalBus bus, PersonStep startPlayerTurnView, AttackMenu attackMenu, EndTurnProcessing endTurnProcessing) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, discover, bus, endTurnProcessing, hand,
                attackMenu, interactionActivator)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _endTurnProcessing = endTurnProcessing;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
            _turnProcessing = turnProcessing;
        }

        public override void StartEffect(Effect effect)
        {
            PushStep(new CardEffectProcessingPlayer(InteractionActivator, effect));

            _turnProcessing.Completed();
        }

        protected override void InitSteps()
        {
            PushStep(_endTurnProcessing);
            PushStep(_turnProcessing);
            PushStep(StartTurnDraw);
            PushStep(_startPlayerTurnView);
        }

        protected override void OnStartStep()
        {
            //_handBlockable.ForciblyBlock();
            //GameFieldObjectsActivator.Activate();
            //_attackMenu.Activate();
        }
    }
}