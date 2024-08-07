using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Discarding;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using StateMachine;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public abstract class Person : IStateMachineState, IDisposable
    {
        private readonly ITurnStep _turnProcess;
        private readonly CardPlayingZone _playingZone;
        private readonly SimpleCardDrawer _drawCardRoot;
        private readonly StartTurnDraw _startTurnDraw;
        private readonly Tower _tower;
        private readonly SignalBus _bus;
        private readonly List<EffectType> _appliedEffects;
        private readonly Queue<ITurnStep> _turnSteps;

        private ITurnStep _currentStep;

        protected Person(CardPlayingZone playingZone, SimpleCardDrawer drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess, SignalBus bus)
        {
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _bus = bus;

            _appliedEffects = new List<EffectType>();
            _turnSteps = new Queue<ITurnStep>();
            
            _bus.Subscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
            _bus.Subscribe<RemoveEffectSignal>(OnRemoveEffectSignal);
        }

        public bool IsComplete { get; private set; }
        public bool IsTowerFilled => _tower.IsTowerFill;

        public void Dispose()
        {
            _bus.Unsubscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
            _bus.Unsubscribe<RemoveEffectSignal>(OnRemoveEffectSignal);
        }

        public void StartStep()
        {
            IsComplete = false;
            
            InitTurnSteps();
            OnStartStep();

            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();
        }

        private void InitTurnSteps()
        {
            _turnSteps.Clear();
            
            EnqueueStep(_startTurnDraw);
            EnqueueStep(_turnProcess);
        }

        public void EffectsIgnoredDrawCards(int countCards, Action callback = null)
            => _drawCardRoot.DrawCards(countCards);

        public void FinishTurn()
        {
            IReadOnlyList<Card> discardedCards = _playingZone.UpdateCards();

            if (discardedCards.Count > 0)
                _bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        protected abstract void OnStartStep();

        private IEnumerator ProcessingTurn()
        {
            while (IsComplete == false)
            {
                _currentStep.StartStep();
                yield return new WaitUntil(() => _currentStep.IsComplete);

                NextStep();
            }
        }

        private void NextStep()
        {
            if (_turnSteps.Count > 0)
            {
                _currentStep = _turnSteps.Dequeue();
            }
            else
            {
                IsComplete = true;
            }
        }

        private void EnqueueStep(ITurnStep turnStep) => _turnSteps.Enqueue(turnStep);

        private void OnEffectCreatedSignal(EffectCreatedSignal signal)
        {
            //if (signal.Target == this)
                _appliedEffects.Add(signal.Type);
        }

        private void OnRemoveEffectSignal(RemoveEffectSignal signal)
        {
            _appliedEffects.Remove(signal.Type);
        }
    }
}