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

        public bool HasFreeSeat => _table.HasFreeSeat;

        public void Init(Table table)
        {
            _table = table;
        }

        public Vector3 GetPosition() => transform.position;

        public void SeatCard(Card card)
        {
            if (HasFreeSeat == false)
                throw new System.Exception("Нет места в " + ToString() + "! Почему не проверил ");

            card.Play();
            _table.SeatCard(card);
            _playedCards.Add(card);
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
