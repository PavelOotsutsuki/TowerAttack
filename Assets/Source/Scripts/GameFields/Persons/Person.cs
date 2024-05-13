using System;
using System.Collections.Generic;
using Cards;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using UnityEngine;

namespace GameFields.Persons
{
    //[Serializable]
    public abstract class Person : IStartTowerCardSelectionListener
    {
        //[SerializeField] protected Hand Hand;
        //[SerializeField] protected Table Table;
        //[SerializeField] protected DrawCardRoot DrawCardRoot;
        //[SerializeField] private Tower _tower;

        protected Hand Hand;
        protected Table Table;
        protected DrawCardRoot DrawCardRoot;
        protected StartTurnDraw StartTurnDraw;

        private Tower _tower;

        private DiscardPile _discardPile;

        public bool IsTowerFilled => _tower.IsTowerFill;

        public Person(Hand hand, Table table, DrawCardRoot drawCardRoot, Tower tower, DiscardPile discardPile, StartTurnDraw startTurnDraw)
        {
            Hand = hand;
            Table = table;
            _tower = tower;
            DrawCardRoot = drawCardRoot;
            StartTurnDraw = startTurnDraw;

            _discardPile = discardPile;
        }

        public abstract void Init();
        public abstract void StartTurn();

        //{
        //    Hand.Init(seatPool);
        //    _tower.Init(Hand);
        //    Table.Init(Hand, effectRoot);

        //    DrawCardRoot.Init(Hand);
        //}


        //public virtual void Init(EffectRoot effectRoot, Deck deck, DiscardPile discardPile, SeatPool seatPool)
        //{
        //    Deck = deck;
        //    _discardPile = discardPile;

        //    Hand.Init(seatPool);
        //    _tower.Init(Hand);
        //    Table.Init(Hand, effectRoot);

        //    DrawCardRoot.Init(Hand);
        //}

        public void DiscardCards()
        {
            _discardPile.DiscardCards(Table.GetDiscardCards());
        }

        public List<Card> DrawCards(int countCards, Action callback = null)
        {
            return DrawCardRoot.DrawCards(countCards, callback);
        }
    }
}
