using Cards;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Zenject;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.InformationLabels;
using GameFields.Persons.EffectCounters;

namespace GameFields.Persons.Common
{
    public class EnemyAI : Person
    {
        //private readonly IDeactivatable _gameFieldObjectsActivator;
        private readonly EnemyDragAndDropImitation _enemyDragAndDropImitation;

        public EnemyAI(InteractionActivator interactionActivator, EnemyDragAndDropImitation enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus,
            Hand hand, ISelectMenuActivator attackMenu, ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation,
            PersonEffectsCounter personEffectsCounter) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw,discoverImitation, bus,
                hand, attackMenu, interactionActivator, choiceMenu, choiceMenuImitation, personEffectsCounter)
        {
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
            _enemyDragAndDropImitation = enemyDragAndDropImitation;

            Bus.Subscribe<AttackSignalEnemyAI>(StartAttack);
        }

        ~EnemyAI()
        {
            Bus.Unsubscribe<AttackSignalEnemyAI>(StartAttack);
        }

        public override void StartAction(ICompletable completable)
        {
            PushStep(new CardActionProcessingEnemyAI(InteractionActivator, completable));
        }

        public override void StartEffect(Effect effect, CardEffectConfig effectConfig)
        {
            base.StartEffect(effect, effectConfig);

            StartAction(effect);
        }

        private void StartAttack(AttackSignalEnemyAI signal)
        {
            StartAction(signal.Completable);
        }

        protected override void InitSteps()
        {
            //PushStep(CardEffectProcessing);
            PushStep(_enemyDragAndDropImitation);
            PushStep(StartTurnDraw);
        }

        //~EnemyAI()
        //{
        //    Bus.Unsubscribe<StartEffectSignal>(SetCardEffectProcess);
        //}

        protected override void OnStartStep()
        {
            //GameFieldObjectsActivator.Deactivate();
        }

        //private void SetCardEffectProcess(StartEffectSignal signal)
        //{
        //    EnqueueStep(new CardEffectProcessing(signal.Card));
        //}
    }
}