using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons
{
    [Serializable]
    public class EnemyAI : IPerson
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private HandAI _hand;
        [SerializeField] private TableAI _table;
        [SerializeField] private TowerAI _tower;

        private IEndTurnHandler _endTurnHandler;
        private CardImitationActions _cardImitationActions;

        public float DrawCardsDelay => _enemyAnimator.DrawCardsDelay;
        public int CountDrawCards => _enemyAnimator.CountDrawCards;

        public void Init(IEndTurnHandler endTurnHandler)
        {
            _endTurnHandler = endTurnHandler;

            _hand.Init();
            _tower.Init(this);
            _table.Init(this);
//            cardEffects.SetEnemyAIGameFieldElements(_table, _hand, _tower);

            _cardImitationActions = new CardImitationActions(_hand, _table);
            _enemyAnimator.Init(_endTurnHandler, _cardImitationActions);
        }

        public void PlayDragAndDropImitation()
        {
            if (_hand.TryGetCard(out Card card))
            {
                _cardImitationActions.SetCard(card);
                _enemyAnimator.StartDragAndDropAnimation();
            }
            else
            {
                _endTurnHandler.OnEndTurn();
            }
        }

        public void DrawCard(Card card)
        {
            _hand.AddCard(card);
        }

        public void PlayCard(Card card)
        {
            _hand.RemoveCard(card);
        }

        public List<Card> GetDiscardCards()
        {
            return _table.GetDiscardCards();
        }
    }
}
