namespace Cards
{
    internal class CardSideFlipper
    {
        private readonly CardFront _front;
        private readonly CardBack _back;
        private readonly CardDragAndDrop _cardDragAndDrop;

        public CardSideFlipper(CardFront front, CardBack back, CardDragAndDrop cardDragAndDrop)
        {
            _front = front;
            _back = back;
            _cardDragAndDrop = cardDragAndDrop;
        }

        public void SetSide(SideType side)
        {
            _front.gameObject.SetActive(side == SideType.Front);
            _back.gameObject.SetActive(side == SideType.Back);
        }

        public void DeactivateInteraction()
        {
            _cardDragAndDrop.enabled = false;
            _front.Block();
        }

        public void ActivateInteraction()
        {
            _cardDragAndDrop.enabled = true;
            _front.Unblock();
        }
    }
}