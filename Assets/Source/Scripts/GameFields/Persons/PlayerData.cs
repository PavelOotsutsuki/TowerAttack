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
    public class PlayerData
    {
        [SerializeField] private HandPlayer _hand;
        [SerializeField] private Table _table;
        [SerializeField] private Tower _tower;
        [SerializeField] private Discover _discover;
        [SerializeField] private int _countStartTurnDrawCards = 1;
        [SerializeField] private DrawCardRoot _drawCardRoot;

        public HandPlayer Hand => _hand;
        public Table Table => _table;
        public Tower Tower  => _tower;
        public Discover Discover => _discover; 
        public int CountStartTurnDrawCards  => _countStartTurnDrawCards; 
        public DrawCardRoot DrawCardRoot => _drawCardRoot;
    }
}
