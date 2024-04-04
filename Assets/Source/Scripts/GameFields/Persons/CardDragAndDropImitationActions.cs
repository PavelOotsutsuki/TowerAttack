using Cards;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private ISeatable _activeCard;

        private ICardDragImitationListener _cardDragImitationListener;
        private ICardPlayPlaceImitation _cardPlayPlaceImitation;

        public CardDragAndDropImitationActions(ICardDragImitationListener cardDragImitationListener, ICardPlayPlaceImitation cardPlayPlaceImitation)
        {
            _cardDragImitationListener = cardDragImitationListener;
            _cardPlayPlaceImitation = cardPlayPlaceImitation;
        }

        internal void SetCard(ISeatable card)
        {
            _activeCard = card;
        }

        public void ViewCard(float duration)
        {
            _activeCard.ViewCard(duration);
        }

        public void PlayOnPlace(float duration)
        {
            _activeCard.PlayOnPlace(_cardPlayPlaceImitation.GetCentralСoordinates(), duration);
            _cardDragImitationListener.OnCardDrag(_activeCard);
        }

        public bool TryReturnToHand(float returnToHandDuration)
        {
            if (_cardPlayPlaceImitation.TrySeatCard(_activeCard) == false)
            {
                _cardDragImitationListener.OnCardDrop();
                _activeCard.ReturnToHand(returnToHandDuration);

                return true;
            }

            return false;
        }
    }
}