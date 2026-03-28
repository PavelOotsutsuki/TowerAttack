using System.Collections.Generic;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields
{
    public class TurnDrawnCards : IDrawnCardAdder, IDrawnCardWatcher, IDrawnCardClearable
    {
        private readonly List<Card> _turnCards;

        public TurnDrawnCards()
        {
            _turnCards = new List<Card>();
        }

        public IEnumerable<Card> DrawnCards => _turnCards;

        public bool IsVoid()
        {
            return _turnCards.Count == 0;
        }

        public void Add(Card card)
        {
            _turnCards.Add(card);
        }

        public void Clear()
        {
            _turnCards.Clear();
        }
    }
}