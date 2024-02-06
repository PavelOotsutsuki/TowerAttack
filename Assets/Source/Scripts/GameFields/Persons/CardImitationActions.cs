using Cards;

namespace GameFields.Persons
{
    public class CardImitationActions
    {
        private Card _activeCard;

        private ICardDragImitationListener _cardDragImitationListener;
        private ICardDropPlaceImitation _cardDropPlaceImitation;

        internal void SetCard(Card card)
        {
            _activeCard = card;
        }

        internal void Reset()
        {
            _activeCard = null;
        }

        public void DragCard()
        {
            _cardDragImitationListener.OnCardDrag(_activeCard);
        }

        public void DropCard()
        {
            _cardDragImitationListener.OnCardDrop();
        }

        public bool TryGetCard()
        {
            return _cardDropPlaceImitation.TryGetCard(_activeCard);
        }

        public void ReturnToHand()
        {
            _activeCard.ReturnToHand();
        }
    }
}