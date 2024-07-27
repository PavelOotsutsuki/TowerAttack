using System.Collections.Generic;
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
        private List<Card> _playedCards;
        private SignalBus _bus;

        public void Init(Table table, IUnbindCardManager unbindCardManager, SignalBus bus)
        {
            _table = table;
            _unbindCardManager = unbindCardManager;
            _bus = bus;
            
            _playedCards = new List<Card>();
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
                _playedCards.Add(card);

                _bus.Fire(new CardPlayedSignal(card));
            }

            return tableHasFreeSeats;
        }

        public IEnumerable<Card> RemoveCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
                _playedCards.Remove(card);
            
            return _table.FreeSeats(cards);
        }
    }
}
