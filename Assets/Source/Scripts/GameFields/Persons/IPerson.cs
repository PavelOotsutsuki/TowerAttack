using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IPerson : IPlayCardManager, IDrawCardManager
    {
        public float DrawCardsDelay { get; }
        public int CountDrawCards { get; }

        public List<Card> GetDiscardCards();
        public TowerCardSelector CreateTowerSelector();
    }
}
