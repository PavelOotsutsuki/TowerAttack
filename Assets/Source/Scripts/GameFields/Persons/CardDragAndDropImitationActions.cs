using Cards;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private Card _activeCard;

        private ICardDragImitationListener _cardDragImitationListener;
        private ICardDropPlaceImitation _cardDropPlaceImitation;

        public CardDragAndDropImitationActions(ICardDragImitationListener cardDragImitationListener, ICardDropPlaceImitation cardDropPlaceImitation)
        {
            _cardDragImitationListener = cardDragImitationListener;
            _cardDropPlaceImitation = cardDropPlaceImitation;
        }

        internal void SetCard(Card card)
        {
            _activeCard = card;
        }

        public void ViewCard(float duration)
        {
            _activeCard.ViewCard(duration);
        }

        public void PlayOnPlace(float duration)
        {
            _activeCard.PlayOnPlace(_cardDropPlaceImitation.GetCentralСoordinates(), duration);
            _cardDragImitationListener.OnCardDrag(_activeCard);
        }

        public bool TryReturnToHand(float returnToHandDuration)
        {
            if (_cardDropPlaceImitation.TrySeatCard(_activeCard) == false)
            {
                _cardDragImitationListener.OnCardDrop();
                _activeCard.ReturnToHand(returnToHandDuration);

                return true;
            }

            return false;
        }
    }
}