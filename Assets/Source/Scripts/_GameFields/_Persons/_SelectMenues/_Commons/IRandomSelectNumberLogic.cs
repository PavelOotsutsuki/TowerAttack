using System.Collections.Generic;

namespace GameFields.Persons.SelectMenues.Commons
{
    public interface IRandomSelectNumberLogic
    {
        public IReadOnlyList<ISelectNumber> GetSelectedNumbers();
    }
}