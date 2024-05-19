using System;
using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IDrawCardManager
    {
        public List<Card> DrawCards(int countCards, Action callback = null);
        //public void StartTurn();
        //public IEnumerator StartTowerCardSelectionDraw();
        //public IEnumerator PatriarchCorallDraw();
    }
}
