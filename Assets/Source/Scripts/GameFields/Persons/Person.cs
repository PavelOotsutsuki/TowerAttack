using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields.Persons
{
    public abstract class Person : IStartTowerCardSelectionListener, IStateMachineState
    {
        private Hand _hand;
        private Table _table;
        private DrawCardRoot _drawCardRoot;
        private StartTurnDraw _startTurnDraw;

        private ITurnStep _turnProcess;

        private Tower _tower;

        private DiscardPile _discardPile;

        private Queue<ITurnStep> _turnSteps;
        private ITurnStep _currentStep;

        private bool _isComplete;

        public Person(Hand hand, Table table, DrawCardRoot drawCardRoot, Tower tower, DiscardPile discardPile,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess)
        {
            _hand = hand;
            _table = table;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;

            _discardPile = discardPile;
        }

        public bool IsComplete => _isComplete;
        public bool IsTowerFilled => _tower.IsTowerFill;

        public void StartStep()
        {
            _isComplete = false;

            _turnSteps = new Queue<ITurnStep>();

            EnqueueStep(_startTurnDraw);
            EnqueueStep(_turnProcess);

            OnStartStep();

            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();
        }

        public void DiscardCards()
        {
            _discardPile.DiscardCards(_table.GetDiscardCards());
        }

        public List<Card> DrawCards(int countCards, Action callback = null)
        {
            return _drawCardRoot.DrawCards(countCards, callback);
        }

        protected abstract void OnStartStep();

        private IEnumerator ProcessingTurn()
        {
            while (_isComplete == false)
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
                _isComplete = true;
            }
        }

        private void EnqueueStep(ITurnStep turnStep)
        {
            _turnSteps.Enqueue(turnStep);
        }
    }
}
