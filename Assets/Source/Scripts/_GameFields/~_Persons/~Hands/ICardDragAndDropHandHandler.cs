using Cards;

namespace GameFields.Persons.Hands
{
    public interface ICardDragAndDropHandHandler
    {
        float ReturnInSeatDuration { get; }

        bool IsDraggable(Card card);
        void OnCardDrag(Card card);
        void OnCardDrop();
        void OnCardPlay();
        //void OnCardAttack();
        void OnCardReturnInHand(Card card);
    }
}