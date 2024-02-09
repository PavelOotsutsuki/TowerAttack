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
    internal class EnemyAI : IPerson
    {
        [SerializeField] private HandAI _hand;
        [SerializeField] private TableAI _table;
        [SerializeField] private TowerAI _tower;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private int _countDrawCardsEnemy = 1;
        [SerializeField] private float _drawCardsDelayEnemy = 0.5f;

        private IEndTurnHandler _endTurnHandler;
        private CardImitationActions _cardImitationActions;

        public void Init(IEndTurnHandler endTurnHandler)
        {
            CountDrawCards = _countDrawCardsEnemy;
            DrawCardsDelay = _drawCardsDelayEnemy;
            _endTurnHandler = endTurnHandler;

            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
            _cardImitationActions = new CardImitationActions(_hand, _table);
            _enemyAnimator.Init(_endTurnHandler, _cardImitationActions);
        }

        public float DrawCardsDelay { get; private set; }
        public int CountDrawCards { get; private set; }

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

        public List<CardCharacter> GetDiscardCards()
        {
            return _table.GetAllCardCharacters();
        }
    }
}
