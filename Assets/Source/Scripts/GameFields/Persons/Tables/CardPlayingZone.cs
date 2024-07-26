using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Discarding;
using GameFields.Effects;
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
        private EffectFactory _effectFactory;

        public void Init(Table table, IUnbindCardManager unbindCardManager, SignalBus bus, EffectFactory effectFactory)
        {
            _table = table;
            _unbindCardManager = unbindCardManager;
            _bus = bus;
            _effectFactory = effectFactory;
            
            _playedCards = new PlayedCards();
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

                Effect effect = _effectFactory.Create(character.Effect);
                _playedCards.Add(new PlayedCardContainer(card, character, effect));
                
                _bus.Fire(new EffectCreatedSignal(effect));
            }

            return tableHasFreeSeats;
        }

        public IEnumerable<Card> GetDiscardedCards()
        {
            //TODO: NEED REFACTOR!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            List<PlayedCardContainer> cards = _playedCards.GetDiscardCards();
            return _table.FreeSeats(cards).Select(container => container.Card).ToList();
        }
    }
}
