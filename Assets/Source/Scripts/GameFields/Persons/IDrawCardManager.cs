using System.Collections;
using Cards;

namespace GameFields.Persons
{
    public interface IDrawCardManager
    {
        public int CountDrawCards { get; }
        public float DrawCardsDelay { get; }

        public void DrawCard(Card[] cards);

        //public void StartTurnDraw();
        //public IEnumerator StartTowerCardSelectionDraw();
        //public IEnumerator PatriarchCorallDraw();
    }
}
