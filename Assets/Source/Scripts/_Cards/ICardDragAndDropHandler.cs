namespace Cards
{
    public interface ICardDragAndDropHandler
    {
        float ReturnInSeatDuration { get; }

        bool IsDraggable(Card card);
        void OnCardDrag(Card card);
        void OnCardDrop();
        void OnCardPlay();
        void OnCardReturnInHand(Card card);
    }
}