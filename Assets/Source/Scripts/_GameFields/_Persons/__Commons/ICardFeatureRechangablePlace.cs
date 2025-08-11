using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.Commons
{
    public interface ICardFeatureRechangablePlace
    {
        public IEnumerable<IFeatureRechanger> GetRechangableCards();
    }
}