namespace Cards
{
    public interface ICardDragAndDropHandler
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