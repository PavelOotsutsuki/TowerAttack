using Cards;
using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons
{
    //[Serializable]
    public class EnemyAI : Person
    {
        //[SerializeField] private EnemyAiData _enemyAiData;

        //[SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private DiscoverImitation _discoverImitation;
        private ITableDeactivator _tableDeactivator;

        //private CardDragAndDropImitationActions _cardDragAndDropImitationActions;

        private Queue<ITurnStep> _turnSteps;
        private ITurnStep _currentStep;

        private bool _isComplete;

        //public bool IsImitationComplete => StartTurnDraw.IsComplete && Imitation.IsComplete && EffectCard.IsComplete;

        public EnemyAI(DiscardPile discardPile, ITableDeactivator tableDeactivator, EnemyDragAndDropImitation enemyDragAndDropImitation, HandAI hand, Table table, Tower tower, DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation, StartTurnDraw startTurnDraw) : base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw)
        {
            _enemyDragAndDropImitation = enemyDragAndDropImitation;
            _discoverImitation = discoverImitation;
            _tableDeactivator = tableDeactivator;

            //_turnSteps = new ITurnStep[]
            //{
            //    startTurnDraw,
            //    _enemyDragAndDropImitation
            //};

            //_cardDragAndDropImitationActions = new CardDragAndDropImitationActions(Hand, Table);
        }

        public override bool IsComplete => _isComplete;

        //public override void Init()
        //{
        //    _enemyDragAndDropImitation.Init(_cardDragAndDropImitationActions, Hand);
        //}
        public override void PrepareToStart()
        {
            _isComplete = false;

            _turnSteps = new Queue<ITurnStep>();

            EnqueueStep(StartTurnDraw);
            EnqueueStep(_enemyDragAndDropImitation);
        }

        public override void StartTurn()
        {
            _tableDeactivator.Deactivate();
            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();
        }

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
