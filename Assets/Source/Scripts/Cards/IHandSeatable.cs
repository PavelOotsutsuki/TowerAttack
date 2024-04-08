namespace Cards
{
    public interface IHandSeatable : ISeatable
    {
        public void SetDragAndDropListener(ICardDragListener cardDragAndDropListener);
        public void ReturnToHand(float duration);
    }
}