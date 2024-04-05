using System.Collections;
using System.Collections.Generic;
using Cards;

namespace GameFields.Persons
{
    public interface IDrawCardManager
    {
        public void DrawCards(Queue<IHandSeatable> cards);
        public void StartTurnDraw();
        //public IEnumerator StartTowerCardSelectionDraw();
        //public IEnumerator PatriarchCorallDraw();
    }
}
