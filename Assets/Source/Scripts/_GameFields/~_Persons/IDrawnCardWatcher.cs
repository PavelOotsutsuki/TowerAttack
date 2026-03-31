using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IDrawnCardWatcher
    {
        public IEnumerable<Card> DrawnCards { get; }
    }
}