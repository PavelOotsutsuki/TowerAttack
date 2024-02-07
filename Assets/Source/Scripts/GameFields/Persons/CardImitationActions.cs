using Cards;
using UnityEngine;

namespace GameFields.Persons
{
    public class CardImitationActions
    {
        private Card _activeCard;

        private ICardDragImitationListener _cardDragImitationListener;
        private ICardDropPlaceImitation _cardDropPlaceImitation;

        public CardImitationActions(ICardDragImitationListener cardDragImitationListener, ICardDropPlaceImitation cardDropPlaceImitation)
        {
            _cardDragImitationListener = cardDragImitationListener;
            _cardDropPlaceImitation = cardDropPlaceImitation;
        }

        internal void SetCard(Card card)
        {
            _activeCard = card;
        }

        internal void Reset()
        {
            _activeCard = null;
        }

        public void ViewCard(ViewType viewType, float duration)
        {
            _activeCard.ViewCard(viewType, duration);
        }

        public void PlayOnPlace(float duration)
        {
            _activeCard.PlayOnPlace(_cardDropPlaceImitation.GetCentralСoordinates(), duration);
            _cardDragImitationListener.OnCardDrag(_activeCard);
        }

        public bool TryGetCard()
        {
            return _cardDropPlaceImitation.TryGetCard(_activeCard);
        }

        public void ReturnToHand(float returnToHandDuration)
        {
            _cardDragImitationListener.OnCardDrop();
            _activeCard.ReturnToHand(returnToHandDuration);
        }
    }
}