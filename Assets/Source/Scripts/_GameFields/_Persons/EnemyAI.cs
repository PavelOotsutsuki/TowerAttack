using GameFields.Persons.AttackMenues;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using UnityEngine;
using Zenject;


namespace GameFields.Persons
{
    public class EnemyAI : Person
    {
        private ITableDeactivator _tableDeactivator;

        public EnemyAI(ITableDeactivator tableDeactivator, ITurnStep enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus,
            Hand hand, AttackMenu attackMenu) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, enemyDragAndDropImitation,discoverImitation, bus, hand, attackMenu)
        {
            _tableDeactivator = tableDeactivator;
            Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        ~EnemyAI()
        {
            Bus.Unsubscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        protected override void OnStartStep()
        {
            _tableDeactivator.Deactivate();
        }

        private void SetCardEffectProcess(StartEffectSignal signal)
        {
            EnqueueStep(new CardEffectProcessing(signal.Card));
        }
    }
}
