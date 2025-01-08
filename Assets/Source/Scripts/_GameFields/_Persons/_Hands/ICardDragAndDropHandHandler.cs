using Cards;

namespace GameFields.Persons.Hands
{
    public interface ICardDragAndDropHandHandler
    {
        bool IsDraggable { get; }
        float ReturnInSeatDuration { get; }

        void OnCardDrag(Card card);
        void OnCardDrop();
        void OnCardPlay();
        void OnCardAttack();
        void OnCardReturnInHand(Card card);
    }
}