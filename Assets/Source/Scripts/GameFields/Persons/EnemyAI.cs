using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System;
using UnityEngine;
using UnityEngine.UI;

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

        public void Init(IEndTurnHandler endTurnHandler, CanvasScaler canvasScaler)
        {
            CountDrawCards = _countDrawCardsEnemy;
            DrawCardsDelay = _drawCardsDelayEnemy;
            _endTurnHandler = endTurnHandler;

            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
            _enemyAnimator.Init(_table, _endTurnHandler, _hand, canvasScaler);
        }

        public float DrawCardsDelay { get; private set; }
        public int CountDrawCards { get; private set; }

        public void PlayDragAndDropImitation()
        {
            if (TryGetHandCard(out Card card))
            {
                Debug.Log("Анимация");
                _enemyAnimator.StartDragAndDropAnimation(card);
            }
            else
            {
                Debug.Log("Конец без анимации");
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

        private bool TryGetHandCard(out Card card)
        {
            if (_hand.TryGetCard(out card))
            {
                return true;
            }

            return false;
        }
    }
}
