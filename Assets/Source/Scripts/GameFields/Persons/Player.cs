using Cards;
using System.Collections.Generic;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.DiscardPiles;
using GameFields.Seats;
using System;
using UnityEngine;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;

namespace GameFields.Persons
{
    //[Serializable]
    internal class Player : Person
    {
        //[SerializeField] private Discover _discover;

        private Discover _discover;
        private ITableActivator _tableActivator;
        private IBlockable _handBlockable;

        public Player(DiscardPile discardPile, ITableActivator tableActivator, HandPlayer hand, Table table, Tower tower, Discover discover, DrawCardRoot drawCardRoot) : base(hand, table, drawCardRoot, tower, discardPile)
        {
            _discover = discover;
            _tableActivator = tableActivator;
            _handBlockable = hand;
        }

        public override void Init()
        {
            _discover.Deactivate();
        }

        public override void StartTurn()
        {
            _handBlockable.Block();
            _tableActivator.Activate();

            DrawCardRoot.StartTurnDraw(_handBlockable.Unblock);
        }
    }
}
