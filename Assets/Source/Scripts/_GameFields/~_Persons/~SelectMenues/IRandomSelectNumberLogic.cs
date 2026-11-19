using System.Collections.Generic;

namespace GameFields.Persons.SelectMenues
{
    public interface IRandomSelectNumberLogic
    {
        public IReadOnlyList<ISelectNumber> GetSelectedNumbers();
    }
}