namespace Cards
{
    public interface ICardTakeListener
    {
        public void OnCardDrag(Card card);
        public void OnCardDrop();
    }
}
