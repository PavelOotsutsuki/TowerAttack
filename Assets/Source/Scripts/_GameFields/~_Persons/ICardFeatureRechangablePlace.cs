using System.Collections.Generic;
using Cards.Views;

namespace GameFields.Persons
{
    public interface ICardFeatureRechangablePlace
    {
        public IEnumerable<IFeatureRechanger> GetRechangableCards();
    }
}