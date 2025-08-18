using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Fires
{
    public class FirePool
    {
        private readonly List<Card> _fireList;

        public FirePool()
        {
            _fireList = new List<Card>();
        }

        public IReadOnlyList<Card> FireList => _fireList;
        public int Count => _fireList.Count;

        public void Add(Card card)
        {
            _fireList.Add(card);
        }

        public void Clear()
        {
            _fireList.Clear();
        }
    }
}