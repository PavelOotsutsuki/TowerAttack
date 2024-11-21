namespace Cards
{
    public interface ICardDragAndDropListener
    {
        public void OnCardDrag(Card card);
        public void OnCardDrop();
        public void OnCardPlay();
        public void OnCardReturnInHand(Card card);
    }
}
