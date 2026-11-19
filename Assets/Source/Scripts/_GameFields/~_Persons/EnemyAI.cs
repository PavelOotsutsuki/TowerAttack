using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Zenject;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EnemyProcessImitations;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.ConfirmableNumbersView;
using System.Threading;
using Servers;

namespace GameFields.Persons
{
    public class EnemyAI : Person, IEnemyAIObject
    {
        //private readonly IDeactivatable _gameFieldObjectsActivator;
        private readonly EnemyDragAndDropImitationCreator _enemyDragAndDropImitationCreator;
        private readonly OnBeforeEndTurnProcessingCreator _onBeforeEndTurnProcessingCreator;
        private readonly StartTurnDrawEnemyAICreator _startTurnDrawEnemyAICreator;
        private readonly EnemySkipTurnViewCreator _enemySkipTurnViewCreator;
        private readonly HandAI _handEnemy;

        public EnemyAI(InteractionActivator interactionActivator, EnemyDragAndDropImitationCreator enemyDragAndDropImitationCreator, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDrawEnemyAICreator startTurnDrawEnemyAICreator, SignalBus bus,
            HandAI hand, ISelectMenuActivator attackMenu, ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation,
            PersonEffectsHandler personEffectsHandler, LookCardMenuEnemyAI lookCardMenu, OnBeforeEndTurnProcessingCreator onBeforeEndTurnProcessingCreator,
            EnemySkipTurnViewCreator enemySkipTurnViewCreator, INumbersStateWatcher numbersStateWatcher, LastSelectedNumbersWatcher lastSelectedNumbersWatcher,
            PersonEffectKeeper personEffectKeeper, TurnToken turnToken, FightProcessDBManager fightProcessDBManager) :
            base(cardPlayingZone, drawCardRoot, tower, discoverImitation, bus,
                hand, attackMenu, interactionActivator, choiceMenu, choiceMenuImitation, personEffectsHandler,
                lookCardMenu, numbersStateWatcher, lastSelectedNumbersWatcher, personEffectKeeper, turnToken, fightProcessDBManager)
        {
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
            _enemyDragAndDropImitationCreator = enemyDragAndDropImitationCreator;
            _onBeforeEndTurnProcessingCreator = onBeforeEndTurnProcessingCreator;
            _startTurnDrawEnemyAICreator = startTurnDrawEnemyAICreator;
            _enemySkipTurnViewCreator = enemySkipTurnViewCreator;
            _handEnemy = hand;

            Bus.Subscribe<PushStepSignalEnemyAI>(StartAttack);
        }

        ~EnemyAI()
        {
            Bus.Unsubscribe<PushStepSignalEnemyAI>(StartAttack);
        }

        public override void StartAction(ICompletable completable)
        {
            //Debug.Log(completable.ToString());
            PushStep(new CardActionProcessingEnemyAI(InteractionActivator, completable, Token));
        }

        //public override void StartEffect(Effect effect, CardEffectConfig effectConfig)
        //{
        //    base.StartEffect(effect, effectConfig);

        //    StartAction(effect);
        //}

        private void StartAttack(PushStepSignalEnemyAI signal)
        {
            StartAction(signal.Completable);
        }

        protected override void InitCommonSteps()
        {
            //PushStep(CardEffectProcessing);
            PushStep(_onBeforeEndTurnProcessingCreator.Create(Token));
            PushStep(_enemyDragAndDropImitationCreator.Create(Token));
            PushStep(_startTurnDrawEnemyAICreator.Create(Token));
            //AddStartTurnDrawStep();
            //PushStep(StartTurnDraw);
        }

        protected override void InitSkipSteps()
        {
            PushStep(_enemySkipTurnViewCreator.Create(Token));
        }

        //~EnemyAI()
        //{
        //    Bus.Unsubscribe<StartEffectSignal>(SetCardEffectProcess);
        //}

        //protected override void OnStartStep()
        //{
        //    //GameFieldObjectsActivator.Deactivate();
        //}

        //public override void ActivateSharpSnakeEffect(Action callback)
        //{
        //    _handEnemy.ActivateSharpSnakeEffect(callback);
        //}

        //private void SetCardEffectProcess(StartEffectSignal signal)
        //{
        //    EnqueueStep(new CardEffectProcessing(signal.Card));
        //}
    }
}