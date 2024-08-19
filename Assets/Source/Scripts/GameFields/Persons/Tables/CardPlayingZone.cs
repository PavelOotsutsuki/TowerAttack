using System.Collections.Generic;
using Cards;
using GameFields.Effects;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour//, ICardDropPlace
    {/*
        private Table _table;
        private IUnbindCardManager _unbindCardManager;
        private EffectRoot _effectRoot;
        private List<Effect> _activeEffects;
        private PlayedCards _playedCards;

        public void Init(IUnbindCardManager unbindCardManager, EffectRoot effectRoot)
        {
            _activeEffects = new List<Effect>();
            _unbindCardManager = unbindCardManager;
            _effectRoot = effectRoot;
        }

        public void EndTurn()
        {
            List<Card> discardCards = _playedCards.GetDiscardCards();
        }

        //public Vector3 GetPosition()
        //{
        //    return transform.position;
        //}

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
            }

            return tableHasFreeSeats;
        }*/
    }
}
