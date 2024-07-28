using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private readonly List<Card> _playedCards = new();
        
        private Table _table;
        private IUnbindCardManager _unbindCardManager;

        public void Init(Table table, IUnbindCardManager unbindCardManager)
        {
            _table = table;
            _unbindCardManager = unbindCardManager;
        }

        public Vector3 GetPosition() => transform.position;

        public bool TrySeatCard(Card card)
        {
            if (_table.HasFreeSeat == false)
                return false;
            
            card.Play();
            _unbindCardManager.UnbindDragableCard();
            _table.SeatCharacter(card.Character);
            _playedCards.Add(card);

            return true;
        }

        public IReadOnlyList<Card> UpdateCards()
        {
            List<Card> toDiscard = new();
            
            foreach (Card playedCard in _playedCards)
            {
                playedCard.DecreaseCounter();

                if (playedCard.EffectCounter <= 0)
                    toDiscard.Add(playedCard);
            }
            
            toDiscard = toDiscard.OrderBy(card => card.Character.transform.position.x).ToList();

            foreach (Card card in toDiscard)
                _playedCards.Remove(card);

            _table.FreeSeats(toDiscard.Select(card => card.Character));
            
            return toDiscard;
        }
    }
}
