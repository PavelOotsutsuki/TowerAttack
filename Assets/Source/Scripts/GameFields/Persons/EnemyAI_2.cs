using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;

namespace GameFields.Persons
{
    internal class EnemyAI_2 : Person_2
    {
        private HandAI_2 _hand;
        private TableAI_2 _table;
        private TowerAI_2 _tower;

        public EnemyAI_2(HandAI_2 hand, TableAI_2 table, TowerAI_2 tower, int countDrawCards)
        {
            CountDrawCards = countDrawCards;
            _hand = hand;
            _table = table;
            _tower = tower;

            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
        }

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
