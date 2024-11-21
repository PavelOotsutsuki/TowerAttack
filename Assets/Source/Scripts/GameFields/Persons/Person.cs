using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Discarding;
using GameFields.Effects;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using StateMachine;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public abstract class Person : IStateMachineState
    {
        private readonly ITurnStep _turnProcess;
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly StartTurnDraw _startTurnDraw;
        private readonly Tower _tower;
        private readonly SignalBus _bus;
        private readonly List<Effect> _appliedEffects;
        private readonly Queue<ITurnStep> _turnSteps;
        private readonly List<Card> _discardedCards;

        private ITurnStep _currentStep;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess, SignalBus bus)
        {
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _bus = bus;

            _appliedEffects = new List<Effect>();
            _turnSteps = new Queue<ITurnStep>();
            _discardedCards = new List<Card>();
            
            _bus.Subscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        public bool IsComplete { get; private set; }
        public bool IsTowerFilled => _tower.IsTowerFill;

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

        public void DrawCards(int countCards, Action callback = null)
            => _drawCardRoot.DrawCards(countCards, callback);

        public void FinishTurn()
        {
            _discardedCards.Clear();
            DecreaseEffectsCounters();

            if (_discardedCards.Count > 0)
            {
                var sortedDiscardedCards = _playingZone.RemoveCards(_discardedCards);
                _bus.Fire(new DiscardCardsSignal(sortedDiscardedCards));
            }
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

        private void DecreaseEffectsCounters()
        {
            List<Effect> effects = new(_appliedEffects);

            foreach (Effect effect in effects)
            {
                effect.DecreaseCounter();
                
                if (effect.CountTurns <= 0)
                    RemoveEffect(effect);
            }
        }

        private void RemoveEffect(Effect effect)
        {
            _discardedCards.Add(effect.Card);
            _appliedEffects.Remove(effect);
        }

        private void OnEffectCreatedSignal(EffectCreatedSignal signal)
        {
            if (signal.Target == this)
                _appliedEffects.Add(signal.Effect);
        }
    }
}