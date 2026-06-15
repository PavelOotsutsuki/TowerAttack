using GameFields.Persons.Discovers;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using Zenject;
using Tools;
using GameFields.Signals;
using GameFields.Persons.SelectMenues;
using GameFields.InformationLabels;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.ConfirmableNumbersView;
using System.Threading;

namespace GameFields.Persons
{
    public class Player : Person, IPlayerObject
    {
        //private readonly IActivatable _gameFieldObjectsActivator;
        private readonly IHandBlockable _handBlockable;
        private readonly ISelectMenuActivator _attackMenu;

        private readonly StartTurnDrawPlayerCreator _startTurnDrawPlayerCreator;
        private readonly TurnProcessingCreator _turnProcessingCreator;
        private readonly StartPlayerTurnViewCreator _startPlayerTurnViewCreator;
        private readonly EndTurnProcessingCreator _endTurnProcessingCreator;
        private readonly PlayerSkipTurnViewCreator _playerSkipTurnViewCreator;

        private readonly InformationLabel _informationLabel;

        private TurnProcessing _currentTurnProcessing;

        public Player(InteractionActivator interactionActivator, HandPlayer hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDrawPlayerCreator startTurnDrawPlayerCreator, TurnProcessingCreator turnProcessingCreator,
            SignalBus bus, StartPlayerTurnViewCreator startPlayerTurnViewCreator, ISelectMenuActivator attackMenu, EndTurnProcessingCreator endTurnProcessingCreator,
            ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation, PersonEffectsHandler personEffectsHandler,
            InformationLabel informationLabel, LookCardMenuPlayer lookCardMenuPlayer, PlayerSkipTurnViewCreator playerSkipTurnViewCreator,
            INumbersStateWatcher numbersStateWatcher, LastSelectedNumbersWatcher lastSelectedNumbersWatcher,
            PersonEffectKeeper personEffectKeeper, CancellationToken fightToken) :
            base(cardPlayingZone, drawCardRoot, tower, discover, bus, hand, attackMenu, interactionActivator,
                choiceMenu, choiceMenuImitation, personEffectsHandler, lookCardMenuPlayer, numbersStateWatcher,
                lastSelectedNumbersWatcher, personEffectKeeper, fightToken)
        {
            _startTurnDrawPlayerCreator = startTurnDrawPlayerCreator;
            _startPlayerTurnViewCreator = startPlayerTurnViewCreator;
            _endTurnProcessingCreator = endTurnProcessingCreator;
            _playerSkipTurnViewCreator = playerSkipTurnViewCreator;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
            _turnProcessingCreator = turnProcessingCreator;

            _informationLabel = informationLabel;

            Bus.Subscribe<PushStepSignalPlayer>(StartAttack);
        }

        ~Player()
        {
            Bus.Unsubscribe<PushStepSignalPlayer>(StartAttack);
        }

        public override void StartAction(ICompletable completable)
        {
            PushStep(new CardActionProcessingPlayer(InteractionActivator, completable, Token));

            _currentTurnProcessing.Completed();
        }

        //public override void StartEffect(Effect effect, CardEffectConfig effectConfig)
        //{
        //    base.StartEffect(effect, effectConfig);

        //    //PushStep(new CardEffectProcessingPlayer(InteractionActivator, effect));
        //    StartAction(effect);

        //    //PushStep(new CardActionProcessingPlayer(InteractionActivator, effect));

        //    //_turnProcessing.Completed();
        //}

        //public override void ActivateSharpSnakeEffect(Action callback)
        //{
        //    ActivatingSharpSnakeEffect(callback).ToUniTask();
        //}

        //private IEnumerator ActivatingSharpSnakeEffect(Action callback)
        //{
        //    LabelActivateData labelActivateData = new LabelActivateData("Соперник смотрит ваши карты...");
        //    InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 10f);
        //    _informationLabel.Activate(informationLabelActivateData);

        //    yield return new WaitUntil(() => _informationLabel.IsComplete);

        //    callback?.Invoke();
        //}

        protected override void InitCommonSteps()
        {
            PushStep(_endTurnProcessingCreator.Create(Token));

            _currentTurnProcessing = _turnProcessingCreator.Create(Token);

            PushStep(_currentTurnProcessing);
            PushStep(_startTurnDrawPlayerCreator.Create(Token));

            //AddStartTurnDrawStep();
            //PushStep(StartTurnDraw);
            PushStep(_startPlayerTurnViewCreator.Create(Token));
        }

        protected override void InitSkipSteps()
        {
            PushStep(_playerSkipTurnViewCreator.Create(Token));
        }

        //protected override void OnStartStep()
        //{
        //    //_handBlockable.ForciblyBlock();
        //    //GameFieldObjectsActivator.Activate();
        //    //_attackMenu.Activate();
        //}

        private void StartAttack(PushStepSignalPlayer signal)
        {
            //PushStep(new CardAttackProcessingPlayer(InteractionActivator, signal.Completable));
            StartAction(signal.Completable);
            //PushStep(new CardActionProcessingPlayer(InteractionActivator, signal.Completable));

            //_turnProcessing.Completed();
        }

    }
}