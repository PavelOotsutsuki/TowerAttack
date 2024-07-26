using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Discarding;
using GameFields.Persons.Hands;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private Table _table;
        private IUnbindCardManager _unbindCardManager;
        private SignalBus _bus;
        private PlayedCards _playedCards;

        public void Init(Table table, IUnbindCardManager unbindCardManager, SignalBus bus)
        {
            _table = table;
            _unbindCardManager = unbindCardManager;
            _bus = bus;
            _playedCards = new PlayedCards();

            _bus.Subscribe<EffectCreatedSignal>(OnEffectCreatedSignal);

        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        private void OnEffectCreatedSignal(EffectCreatedSignal signal)
        {
            if (_playedCards.HasCharacter(signal.Character))
                _playedCards.BindEffect(signal.Character, signal.Effect);
        }

        public Vector3 GetPosition() => transform.position;

        public bool TrySeatCard(Card card)
        {
            bool tableHasFreeSeats = _table.HasFreeSeat;

            if (tableHasFreeSeats)
            {
                CardCharacter character = card.Play();
                _unbindCardManager.UnbindDragableCard();
                _table.SeatCharacter(character);

                _playedCards.Add(character, card);
                _bus.Fire(new CardPlayedSignal(character));
            }

            return tableHasFreeSeats;
        }

        public List<Card> GetDiscardedCards()
        {
            Dictionary<CardCharacter, Card> cards = _playedCards.GetDiscardCards();
            //List<CardCharacter> characters = cards.Select(card => _playedCards.GetCharacterByCard(card)).ToList();

            //foreach (CardCharacter character in cards.Keys)
            //    _table.FreeSeatBySeatable(character);

            //_playedCards.RemoveCard(cards.Values);

            return _table.FreeSeats(cards).Values.ToList();
        }
    }
}
