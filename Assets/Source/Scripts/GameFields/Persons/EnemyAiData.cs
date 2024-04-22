using System;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields
{
    [Serializable]
    public class EnemyAiData
    {
        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private HandAI _hand;
        [SerializeField] private TableAI _table;
        [SerializeField] private Tower _tower;
        [SerializeField] private DrawCardRoot _drawCardRoot;

        public EnemyDragAndDropImitation EnemyDragAndDropImitation => _enemyDragAndDropImitation;
        public HandAI Hand  => _hand; 
        public TableAI Table => _table;
        public Tower Tower => _tower;
        public DrawCardRoot DrawCardRoot => _drawCardRoot;
    }
}
