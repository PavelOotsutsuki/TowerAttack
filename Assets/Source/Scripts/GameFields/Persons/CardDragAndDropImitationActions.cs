using Cards;
using Tools;
using UnityEngine;

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
                ICardTransformable cardTransformable = _activeCard;
                Movement movement = new Movement(cardTransformable.Transform);
                _cardDragImitationListener.OnCardDrop();
                movement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, cardTransformable.DefaultScaleVector);

                return true;
            }

            return false;
        }
    }
}