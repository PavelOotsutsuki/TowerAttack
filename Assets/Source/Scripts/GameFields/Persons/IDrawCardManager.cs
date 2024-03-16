using System.Collections;
using Cards;

namespace GameFields.Persons
{
    public interface IDrawCardManager
    {
        //public float DrawCardsDelay { get; }

        //public void DrawCard(Card card);

        public void StartTurnDraw();
        public IEnumerator StartTowerCardSelectionDraw();
        public IEnumerator PatriarchCorallDraw();
    }
}
