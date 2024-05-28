using Cards;
using System.Collections.Generic;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.DiscardPiles;
using GameFields.Seats;
using System;
using UnityEngine;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons
{
    //[Serializable]
    internal class Player : Person
    {
        //[SerializeField] private Discover _discover;

        private Discover _discover;
        private ITableActivator _tableActivator;
        private IBlockable _handBlockable;
        private TurnProcessing _turnProcessing;

        private Queue<ITurnStep> _turnSteps;
        private ITurnStep _currentStep;

        private bool _isComplete;

        public Player(DiscardPile discardPile, ITableActivator tableActivator, HandPlayer hand, Table table, Tower tower, Discover discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, TurnProcessing turnProcessing) : base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw)
        {
            _discover = discover;
            _tableActivator = tableActivator;
            _handBlockable = hand;
            _turnProcessing = turnProcessing;

            _discover.Deactivate();
        }

        public override bool IsComplete => _isComplete;

        //public override void Init()
        //{
        //    _discover.Deactivate();
        //}

        public override void PrepareToStart()
        {
            _isComplete = false;

            _turnSteps = new Queue<ITurnStep>();

            EnqueueStep(StartTurnDraw);
            EnqueueStep(_turnProcessing);
        }

        public override void StartTurn()
        {
            _handBlockable.Block();
            _tableActivator.Activate();

            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();


            //DrawCardRoot.StartTurnDraw(_handBlockable.Unblock);
        }

        //private IEnumerator ProcessingTurn()
        //{
        //    StartTurnDraw.PrepareToStart();
        //    StartTurnDraw.StartStep();

        //    yield return new WaitUntil(()=> StartTurnDraw.IsComplete);

        //    _handBlockable.Unblock();
        //}

        private IEnumerator ProcessingTurn()
        {
            //StartTurnDraw.PrepareToStart();
            //StartTurnDraw.StartStep();

            //yield return new WaitUntil(() => StartTurnDraw.IsComplete);

            //PlayDragAndDropImitation();

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

            turnStep.PrepareToStart();
        }
    }
}