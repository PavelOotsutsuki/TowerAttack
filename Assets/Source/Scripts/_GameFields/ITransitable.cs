using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields
{
    public interface ITransitable 
    {
        public bool SeatCard(Card card);
        public bool TryGetCard(Card card);
        public bool TryGetAllCards(out IEnumerable<Card> cards);
    }
}
