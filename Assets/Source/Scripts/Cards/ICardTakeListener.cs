namespace Cards
{
    public interface ICardTakeListener
    {
        public void OnCardDrag(ISeatable seatableCard);
        public void OnCardDrop();
    }
}
