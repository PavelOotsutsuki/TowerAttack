using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IPerson : IStartTowerCardSelectionListener
    {
        //public int CountDrawCards { get; }

        //public List<Card> GetDiscardCards();
        public void DiscardCards();
    }
}
