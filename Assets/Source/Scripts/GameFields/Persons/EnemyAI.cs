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

        private CardDragAndDropImitationActions _cardDragAndDropImitationActions;

        private bool _isComplete;

        public bool IsComplete => _isComplete;
        //public bool IsImitationComplete => StartTurnDraw.IsComplete && Imitation.IsComplete && EffectCard.IsComplete;

        public EnemyAI(DiscardPile discardPile, ITableDeactivator tableDeactivator, EnemyDragAndDropImitation enemyDragAndDropImitation, HandAI hand, Table table, Tower tower, DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation, StartTurnDraw startTurnDraw) : base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw)
        {
            _enemyDragAndDropImitation = enemyDragAndDropImitation;
            _discoverImitation = discoverImitation;
            _tableDeactivator = tableDeactivator;

            _cardDragAndDropImitationActions = new CardDragAndDropImitationActions(Hand, Table);
        }

        public override void Init()
        {
            _enemyDragAndDropImitation.Init(_cardDragAndDropImitationActions, CompleteImitation);
        }

        public override void StartTurn()
        {
            _tableDeactivator.Deactivate();

            _isComplete = false;

            //if (cards.Count > 0)
            //{
            ProcessingTurn().ToUniTask();
            //}
            //else
            //{
            //    PlayDragAndDropImitation();
            //}
        }

        private void PlayDragAndDropImitation()
        {
            if (Hand.TryGetCard(out Card card))
            {
                _cardDragAndDropImitationActions.SetCard(card);
                _enemyDragAndDropImitation.StartDragAndDropAnimation();
            }
            else
            {
                CompleteImitation();
            }
        }

        private void CompleteImitation()
        {
            _isComplete = true;
        }

        private IEnumerator ProcessingTurn()
        {
            StartTurnDraw.PrepareToStart();
            StartTurnDraw.StartStep();

            yield return new WaitUntil(() => StartTurnDraw.IsComplete);

            PlayDragAndDropImitation();
        }
    }
}
