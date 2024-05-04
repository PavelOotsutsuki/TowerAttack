namespace Cards
{
    public interface ICardDragListener
    {
        public void OnCardDrag(Card card);
        public void OnCardDrop();
        public void OnCardPlay();
    }
}
