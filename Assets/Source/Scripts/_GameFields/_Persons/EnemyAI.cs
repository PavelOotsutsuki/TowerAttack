using Cards;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Zenject;

namespace GameFields.Persons
{
    public class EnemyAI : Person
    {
        //private readonly IDeactivatable _gameFieldObjectsActivator;
        private readonly EnemyDragAndDropImitation _enemyDragAndDropImitation;

        public EnemyAI(InteractionActivator interactionActivator, EnemyDragAndDropImitation enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus,
            Hand hand, IAttackMenu attackMenu) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw,discoverImitation, bus,
                hand, attackMenu, interactionActivator)
        {
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
            _enemyDragAndDropImitation = enemyDragAndDropImitation;
        }

        public override void StartEffect(Effect effect)
        {
            PushStep(new CardEffectProcessingEnemyAI(InteractionActivator, effect));
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