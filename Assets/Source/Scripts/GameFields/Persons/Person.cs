using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.Persons.Discovers;
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
        private readonly Queue<ITurnStep> _turnSteps;
        private readonly Discover _discover;

        private ITurnStep _currentStep;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess, Discover discover, SignalBus bus)
        {
            _bus = bus;
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _discover = discover;

            _turnSteps = new Queue<ITurnStep>();

            _discover.Deactivate();
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

        public List<Card> DrawCards(int countCards, Action callback = null)
            => _drawCardRoot.DrawCards(countCards, callback);

        public void FinishTurn()
        {
            IReadOnlyList<Card> discardedCards = _playingZone.UpdateCards();

            if (discardedCards.Count > 0)
                _bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        public void DiscoverCards(List<Card> cards, string activateMessage)
        {
            if (cards.Count > _discover.MaxSeats)
            {
                throw new Exception("So many cards for discover: " + cards.Count + "/" + _discover.MaxSeats);
            }

            _discover.Activate(cards, activateMessage);
        }

        protected abstract void OnStartStep();

        private void InitTurnSteps()
        {
            _turnSteps.Clear();
            
            EnqueueStep(_startTurnDraw);
            EnqueueStep(_turnProcess);
        }

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
    }
}