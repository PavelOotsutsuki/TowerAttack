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
using GameFields.Persons.EffectHandlers;
using System;
using Tools.UI;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.Commons
{
    public class Player : Person
    {
        //private readonly IActivatable _gameFieldObjectsActivator;
        private readonly IHandBlockable _handBlockable;
        private readonly PersonStep _startPlayerTurnView;
        private readonly EndTurnProcessing _endTurnProcessing;

        private readonly InformationLabel _informationLabel;

        private readonly TurnProcessing _turnProcessing;

        private ISelectMenuActivator _attackMenu;

        public Player(InteractionActivator interactionActivator, HandPlayer hand, CardPlayingZone cardPlayingZone, Tower tower,
            DiscoverPlayer discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, TurnProcessing turnProcessing,
            SignalBus bus, PersonStep startPlayerTurnView, ISelectMenuActivator attackMenu, EndTurnProcessing endTurnProcessing,
            ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation, PersonEffectsHandler personEffectsHandler,
            InformationLabel informationLabel) :
            base(cardPlayingZone, drawCardRoot, tower, startTurnDraw, discover, bus, hand,
                attackMenu, interactionActivator, choiceMenu, choiceMenuImitation, personEffectsHandler)
        {
            _startPlayerTurnView = startPlayerTurnView;
            _endTurnProcessing = endTurnProcessing;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
            _handBlockable = hand;
            _attackMenu = attackMenu;
            _turnProcessing = turnProcessing;

            _informationLabel = informationLabel;

            Bus.Subscribe<PushStepSignalPlayer>(StartAttack);
        }

        ~Player()
        {
            Bus.Unsubscribe<PushStepSignalPlayer>(StartAttack);
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

        public override void ActivateSharpSnakeEffect(Action callback)
        {
            ActivatingSharpSnakeEffect(callback).ToUniTask();
        }

        private IEnumerator ActivatingSharpSnakeEffect(Action callback)
        {
            LabelActivateData labelActivateData = new LabelActivateData("Соперник смотрит ваши карты...");
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 10f);
            _informationLabel.Activate(informationLabelActivateData);

            yield return new WaitUntil(() => _informationLabel.IsComplete);

            callback?.Invoke();
        }

        protected override void InitSteps()
        {
            PushStep(_endTurnProcessing);
            PushStep(_turnProcessing);
            AddStartTurnDrawStep();
            //PushStep(StartTurnDraw);
            PushStep(_startPlayerTurnView);
        }

        protected override void OnStartStep()
        {
            //_handBlockable.ForciblyBlock();
            //GameFieldObjectsActivator.Activate();
            //_attackMenu.Activate();
        }

        private void StartAttack(PushStepSignalPlayer signal)
        {
            //PushStep(new CardAttackProcessingPlayer(InteractionActivator, signal.Completable));
            StartAction(signal.Completable);
            //PushStep(new CardActionProcessingPlayer(InteractionActivator, signal.Completable));

            //_turnProcessing.Completed();
        }

    }
}