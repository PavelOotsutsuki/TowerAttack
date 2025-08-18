using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;
using GameFields.Persons.SelectMenues.Attacks;
using Tools;
using Cards;
using GameFields.Signals;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.InformationLabels;

namespace GameFields.Persons.Common
{
    public class Player : Person
    {
        //private readonly IActivatable _gameFieldObjectsActivator;
        private readonly IHandBlockable _handBlockable;
        private readonly PersonStep _startPlayerTurnView;
        private readonly EndTurnProcessing _endTurnProcessing;

        private readonly TurnProcessing _turnProcessing;

        private ISelectMenuActivator _attackMenu;

        public Player(InteractionActivator interactionActivator, Hand hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, TurnProcessing turnProcessing,
            SignalBus bus, PersonStep startPlayerTurnView, ISelectMenuActivator attackMenu, EndTurnProcessing endTurnProcessing,
            ISelectMenuActivator selectMenu) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, discover, bus, hand,
                attackMenu, interactionActivator, selectMenu)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _endTurnProcessing = endTurnProcessing;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
            _turnProcessing = turnProcessing;

            Bus.Subscribe<AttackSignalPlayer>(StartAttack);
        }

        ~Player()
        {
            Bus.Unsubscribe<AttackSignalPlayer>(StartAttack);
        }

        public override void StartAction(ICompletable completable)
        {
            PushStep(new CardActionProcessingPlayer(InteractionActivator, completable));

            _turnProcessing.Completed();
        }

        public override void StartEffect(Effect effect, CardEffectConfig effectConfig)
        {
            base.StartEffect(effect, effectConfig);

            //PushStep(new CardEffectProcessingPlayer(InteractionActivator, effect));
            StartAction(effect);

            //PushStep(new CardActionProcessingPlayer(InteractionActivator, effect));

            //_turnProcessing.Completed();
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

        private void StartAttack(AttackSignalPlayer signal)
        {
            //PushStep(new CardAttackProcessingPlayer(InteractionActivator, signal.Completable));
            StartAction(signal.Completable);
            //PushStep(new CardActionProcessingPlayer(InteractionActivator, signal.Completable));

            //_turnProcessing.Completed();
        }

    }
}