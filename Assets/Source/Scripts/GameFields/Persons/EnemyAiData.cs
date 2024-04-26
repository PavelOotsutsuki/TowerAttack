using System;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields.Persons
{
    [Serializable]
    public class EnemyAiData
    {
        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private HandAI _hand;
        [SerializeField] private Table _table;
        [SerializeField] private Tower _tower;
        [SerializeField] private DrawCardRoot _drawCardRoot;
        [SerializeField] private DiscoverImitation _discoverImitation;

        public EnemyDragAndDropImitation EnemyDragAndDropImitation => _enemyDragAndDropImitation;
        public HandAI Hand  => _hand; 
        public Table Table => _table;
        public Tower Tower => _tower;
        public DrawCardRoot DrawCardRoot => _drawCardRoot;
        public DiscoverImitation DiscoverImitation => _discoverImitation;
    }
}
