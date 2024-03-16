using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IPerson : IPlayCardManager, IDrawCardManager
    {
        //public int CountDrawCards { get; }

        public List<Card> GetDiscardCards();
        public void ActivateStartTowerCardSelection();
    }
}
