using Cards;
using System.Collections.Generic;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.DiscardPiles;
using GameFields.Seats;
using System;
using UnityEngine;
using GameFields.Persons.Tables;

namespace GameFields.Persons
{
    //[Serializable]
    internal class Player : Person
    {
        //[SerializeField] private Discover _discover;

        private Discover _discover;
        private int _countStartTurnDrawCards;
        private ITableActivator _tableActivator;

        public Player(Deck deck, DiscardPile discardPile, ITableActivator tableActivator, PlayerData playerData): base(playerData.Hand, playerData.Table, playerData.DrawCardRoot, playerData.Tower, deck, discardPile)
        {
            _discover = playerData.Discover;
            _tableActivator = tableActivator;
            _countStartTurnDrawCards = playerData.CountStartTurnDrawCards;
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
