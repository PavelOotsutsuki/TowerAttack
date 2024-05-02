using Cards;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using System;
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

        private bool _isImitationComplete;

        public bool IsImitationComplete => _isImitationComplete;

        public EnemyAI(DiscardPile discardPile, ITableDeactivator tableDeactivator, EnemyDragAndDropImitation enemyDragAndDropImitation, HandAI hand, Table table, Tower tower, DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation) : base(hand, table, drawCardRoot, tower, discardPile)
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

            _isImitationComplete = false;

            //if (cards.Count > 0)
            //{
            DrawCardRoot.StartTurnDraw(PlayDragAndDropImitation);
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
            _isImitationComplete = true;
        }
    }
}
