using System;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private Table _table;
        private IUnbindCardManager _unbindCardManager;

        public event Action<Card, CardCharacter> Played;

        public void Init(Table table, IUnbindCardManager unbindCardManager)
        {
            _table = table;
            _unbindCardManager = unbindCardManager;
        }

        public void FreeSeatsByCharacters(List<CardCharacter> characters)
        {
            foreach (CardCharacter character in characters)
                _table.FreeSeatByCharacter(character.gameObject);
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

                Played?.Invoke(card, character);
            }

            return tableHasFreeSeats;
        }
    }
}
