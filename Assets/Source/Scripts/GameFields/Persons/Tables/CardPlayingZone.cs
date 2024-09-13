using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private readonly List<Card> _playedCards = new List<Card>();
        
        private Table _table;

        public void Init(Table table)
        {
            _table = table;
        }

        public Vector3 GetPosition() => transform.position;

        public bool TrySeatCard(Card card)
        {
            if (_table.HasFreeSeat == false)
                return false;
            
            card.Play();
            _table.SeatCard(card);
            _playedCards.Add(card);

            return true;
        }

        public IReadOnlyList<Card> UpdateCards()
        {
            List<Card> toDiscard = new List<Card>();
            
            foreach (Card playedCard in _playedCards)
            {
                if (playedCard.TryDiscard())
                {
                    toDiscard.Add(playedCard);
                }
            }
            
            toDiscard = toDiscard.OrderBy(card => card.transform.position.x).ToList();

            foreach (Card card in toDiscard)
                _playedCards.Remove(card);

            _table.FreeSeats(toDiscard.Select(card => card));
            
            return toDiscard;
        }
    }
}
