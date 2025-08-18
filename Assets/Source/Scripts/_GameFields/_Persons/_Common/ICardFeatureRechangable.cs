using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Common
{
    public interface ICardFeatureRechangable
    {
        public IEnumerable<IFeatureRechanger> GetRechangableCards();
    }
}