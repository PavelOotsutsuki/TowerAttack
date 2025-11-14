using System.Collections.Generic;
using Cards.Views;

namespace GameFields.Persons.Commons
{
    public interface ICardFeatureRechangablePlace
    {
        public IEnumerable<IFeatureRechanger> GetRechangableCards();
    }
}