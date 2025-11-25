using System.Collections.Generic;
using Cards;

namespace GameFields
{
    public interface ICardView: ICardCheck
    {
        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions);
        public IEnumerable<Card> AllCards { get; }
        public bool Contains(int number);
        public int IndexOf(Card card);
    }
}