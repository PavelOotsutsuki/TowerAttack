using System.Collections.Generic;

namespace GameFields
{
    public interface ICardCheck
    {
        public bool IsHasCards(int count, IEnumerable<int> exceptions = null);
    }
}