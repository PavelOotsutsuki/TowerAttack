using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Discarding;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private readonly List<Card> _playedCards = new List<Card>();
        
        private Table _table;
        //private SignalBus _bus;

        public void Init(Table table/*, SignalBus bus*/)
        {
            _table = table;
            //_bus = bus;
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

                //if (playedCard.EffectCounter <= 0)
                //{
                //    toDiscard.Add(playedCard);
                //    _bus.Fire(new RemoveEffectSignal(playedCard.EffectType));
                //}
            }
            
            toDiscard = toDiscard.OrderBy(card => card.transform.position.x).ToList();

            foreach (Card card in toDiscard)
                _playedCards.Remove(card);

            _table.FreeSeats(toDiscard.Select(card => card));
            
            return toDiscard;
        }
    }
}
