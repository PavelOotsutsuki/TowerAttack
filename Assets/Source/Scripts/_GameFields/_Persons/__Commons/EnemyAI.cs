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
using GameFields.Persons.EffectHandlers;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.Commons
{
    public class EnemyAI : Person
    {
        //private readonly IDeactivatable _gameFieldObjectsActivator;
        private readonly EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private readonly HandAI _handEnemy;

        public EnemyAI(InteractionActivator interactionActivator, EnemyDragAndDropImitation enemyDragAndDropImitation, CardPlayingZone cardPlayingZone,
            Tower tower, DrawCardRoot drawCardRoot, DiscoverAI discoverImitation, StartTurnDraw startTurnDraw, SignalBus bus,
            HandAI hand, ISelectMenuActivator attackMenu, ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation,
            PersonEffectsHandler personEffectsHandler) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw,discoverImitation, bus,
                hand, attackMenu, interactionActivator, choiceMenu, choiceMenuImitation, personEffectsHandler)
        {
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
            _enemyDragAndDropImitation = enemyDragAndDropImitation;
            _handEnemy = hand;

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

        public override void ActivateSharpSnakeEffect(Action callback)
        {
            _handEnemy.ActivateSharpSnakeEffect(callback);
        }

        //private void SetCardEffectProcess(StartEffectSignal signal)
        //{
        //    EnqueueStep(new CardEffectProcessing(signal.Card));
        //}
    }
}