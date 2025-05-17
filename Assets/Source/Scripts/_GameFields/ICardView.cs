using System.Collections.Generic;
using Cards;

namespace GameFields
{
    public interface ICardView: ICardCheck
    {
        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions);
    }
}