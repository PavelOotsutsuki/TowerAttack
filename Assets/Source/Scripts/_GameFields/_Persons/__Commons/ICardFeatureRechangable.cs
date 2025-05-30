using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Commons
{
    public interface ICardFeatureRechangable
    {
        public IEnumerable<IFeatureRechanger> GetRechangableCards();
    }
}