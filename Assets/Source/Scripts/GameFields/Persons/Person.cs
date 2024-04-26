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

        protected Deck Deck;

        private Tower _tower;

        private DiscardPile _discardPile;

        public bool IsTowerFilled => _tower.IsTowerFill;

        public Person(Hand hand, Table table, DrawCardRoot drawCardRoot, Tower tower, Deck deck, DiscardPile discardPile)
        {
            Hand = hand;
            Table = table;
            DrawCardRoot = drawCardRoot;
            _tower = tower;

            Deck = deck;
            _discardPile = discardPile;
        }

        public virtual void Init(EffectRoot effectRoot, SeatPool seatPool)
        {
            Hand.Init(seatPool);
            _tower.Init(Hand);
            Table.Init(Hand, effectRoot);

            DrawCardRoot.Init(Hand);
        }


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

        public void DrawCards(Queue<Card> cards)
        {
            DrawCardRoot.TakeCards(cards);
        }

        public void StartTowerCardSelection(int drawCardsCount)
        {
            Queue<Card> cards = new Queue<Card>();

            for (int i = 0; i < drawCardsCount; i++)
            {
                if (Deck.IsHasCards(1))
                {
                    cards.Enqueue(Deck.TakeTopCard());
                }
            }

            if (cards.Count > 0)
            {
                DrawCardRoot.TakeCards(cards);
            }
        }

        public abstract void StartTurn();
    }
}
