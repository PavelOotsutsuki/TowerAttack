using System.Collections.Generic;
using Cards;
using GameFields.DiscardPiles;
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
        private EffectRoot _effectRoot;
        private List<Effect> _activeEffects;
        private PlayedCards _playedCards;
        private DiscardPile _discardPile;

        private SignalBus _bus;

        [Inject]
        private void Construct(SignalBus bus)
        {
            _bus.DeclareSignal<PlayCardSignal>();
            _bus = bus;
        }

        public void Init(IUnbindCardManager unbindCardManager, EffectRoot effectRoot, DiscardPile discardPile)
        {
            _activeEffects = new List<Effect>();
            _unbindCardManager = unbindCardManager;
            _effectRoot = effectRoot;
            _discardPile = discardPile;
        }

        public void EndTurn()
        {
            List<Card> discardCards = _playedCards.GetDiscardCards();

            foreach (Effect effect in _activeEffects)
            {
                effect.DecreaseCounter();

                if (effect.CountTurns <= 0)
                {
                    _activeEffects.Remove(effect);
                }
            }

            if (discardCards.Count > 0)
            {
                _discardPile.DiscardCards(discardCards);
            }
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }

        public bool TrySeatCard(Card card)
        {
            bool tableHasFreeSeats = _table.IsHasFreeSeat();

            if (tableHasFreeSeats)
            {
                CardCharacter character = card.Play();
                Effect effect = _effectRoot.PlayEffect(character.Effect);
                _playedCards.Add(character, card, effect.CountTurns);
                _unbindCardManager.UnbindDragableCard();
                _table.SeatCardCharacter(character); 
                _activeEffects.Add(effect);

                _bus.Fire(new PlayCardSignal(character.Effect));
            }

            return tableHasFreeSeats;
        }
    }
}
