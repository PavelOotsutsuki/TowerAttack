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
        private int _countStartTurnDrawCards;
        private ITableActivator _tableActivator;

        public Player(Deck deck, DiscardPile discardPile, ITableActivator tableActivator, HandPlayer hand, Table table, Tower tower, Discover discover, int countStartTurnDrawCards, DrawCardRoot drawCardRoot) : base(hand, table, drawCardRoot, tower, deck, discardPile)
        {
            _discover = discover;
            _tableActivator = tableActivator;
            _countStartTurnDrawCards = countStartTurnDrawCards;
        }

        public override void Init(EffectRoot effectRoot, SeatPool seatPool)
        {
            base.Init(effectRoot, seatPool);

            _discover.Deactivate();
        }

        public override void StartTurn()
        {
            _tableActivator.Activate();

            Queue<Card> cards = new Queue<Card>();

            for (int i = 0; i < _countStartTurnDrawCards; i++)
            {
                if (Deck.IsHasCards(1))
                {
                    cards.Enqueue(Deck.TakeTopCard());
                }
            }

            if (cards != null)
            {
                DrawCardRoot.TakeCards(cards);
            }
        }
    }
}
