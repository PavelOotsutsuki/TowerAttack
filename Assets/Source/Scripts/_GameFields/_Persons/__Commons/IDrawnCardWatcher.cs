using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Commons
{
    public interface IDrawnCardWatcher
    {
        public IEnumerable<Card> DrawnCards { get; }
    }
}