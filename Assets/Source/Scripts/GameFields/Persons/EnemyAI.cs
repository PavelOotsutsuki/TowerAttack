using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System;
using System.Collections.Generic;

namespace GameFields.Persons
{
    [Serializable]
    public class EnemyAI : IPerson
    {
        private EnemyAnimator _enemyAnimator;
        private HandAI _hand;
        private TableAI _table;
        private TowerAI _tower;
        private IEndTurnHandler _endTurnHandler;
        private CardImitationActions _cardImitationActions;

        public EnemyAI(EnemyAnimator enemyAnimator, HandAI hand, TableAI table, TowerAI tower)
        {
            _enemyAnimator = enemyAnimator;
            _hand = hand;
            _table = table;
            _tower = tower;
        }

        public float DrawCardsDelay => _enemyAnimator.DrawCardsDelay;
        public int CountDrawCards => _enemyAnimator.CountDrawCards;

        public void Init(IEndTurnHandler endTurnHandler, CardEffects cardEffects)
        {
            _endTurnHandler = endTurnHandler;

            _hand.Init();
            _tower.Init(this);
            _table.Init(this, cardEffects);
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
