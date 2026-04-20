using System.Collections.Generic;

namespace Cards
{
    public interface ICardWatcher
    {
        public IReadOnlyList<Card> Cards { get; }
    }
}