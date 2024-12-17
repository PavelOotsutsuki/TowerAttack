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

        public EnemyAI(GameFieldObjectsActivator gameFieldObjectsActivator, PersonStep enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus,
            Hand hand, AttackMenu attackMenu) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, enemyDragAndDropImitation,discoverImitation, bus, hand,
                attackMenu, gameFieldObjectsActivator)
        {
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        protected override void InitSteps()
        {
            EnqueueStep(StartTurnDraw);
            EnqueueStep(TurnProcess);
            EnqueueStep(CardEffectProcessing);
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