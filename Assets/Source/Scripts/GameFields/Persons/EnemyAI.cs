using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System;
using UnityEngine;

namespace GameFields.Persons
{
    [Serializable]
    internal class EnemyAI : IPerson
    {
        [SerializeField] private HandAI _hand;
        [SerializeField] private TableAI _table;
        [SerializeField] private TowerAI _tower;
        [SerializeField] private int _countDrawCardsEnemy = 1;
        [SerializeField] private float _drawCardsDelayEnemy = 0.5f;

        public void Init()
        {
            CountDrawCards = _countDrawCardsEnemy;
            DrawCardsDelay = _drawCardsDelayEnemy;

            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
        }

        public float DrawCardsDelay { get; private set; }
        public int CountDrawCards { get; private set; }

        public void DrawCard(Card card)
        {
            _hand.AddCard(card);
        }

        public void PlayCard(Card card)
        {

        }
    }
}
