using Cards;
using GameFields.Effects;
using GameFields.Persons.Hands;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System;
using System.Collections;
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
        private CardDragAndDropImitationActions _cardDragAndDropImitationActions;

        public float DrawCardsDelay => _enemyAnimator.DrawCardsDelay;
        public int CountDrawCards => _enemyAnimator.CountDrawCards;

        public void Init(IEndTurnHandler endTurnHandler, EffectRoot effectRoot)
        {
            _endTurnHandler = endTurnHandler;

            _hand.Init();
            _tower.Init(this);
            _table.Init(this, effectRoot);
//            cardEffects.SetEnemyAIGameFieldElements(_table, _hand, _tower);

            _cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_hand, _table);
            _enemyAnimator.Init(_endTurnHandler, _cardDragAndDropImitationActions);
        }

        public void PlayDragAndDropImitation()
        {
            if (_hand.TryGetCard(out Card card))
            {
                _cardDragAndDropImitationActions.SetCard(card);
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

        public void StartTurnDraw()
        {
            
        }

        public IEnumerator StartTowerCardSelectionDraw()
        {
            yield break;
        }

        public IEnumerator PatriarchCorallDraw()
        {
            yield break;
        }
    }
}
