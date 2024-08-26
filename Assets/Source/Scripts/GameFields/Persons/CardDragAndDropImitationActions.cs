using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private Card _activeCard;
        private RectTransform _cardTransform;
        private CardMovement _cardMovement;

        private ICardDragAndDropListener _cardDragAndDropListener;
        private ICardDropPlace _cardDropPlaceImitation;

        public CardDragAndDropImitationActions(ICardDragAndDropListener cardDragListener, ICardDropPlace cardDropPlaceImitation)
        {
            _cardDragAndDropListener = cardDragListener;
            _cardDropPlaceImitation = cardDropPlaceImitation;
        }

        internal void SetCard(Card card)
        {
            _activeCard = card;
            _cardTransform = _activeCard.Transform;
            _cardMovement = _activeCard.CardMovement;
        }

        public void ViewCard(float duration, float yDirection)
        {
            Vector3 position = _cardTransform.localPosition;
            position.y += _cardTransform.rect.height / 2 * yDirection;

            _cardMovement.MoveLocalSmoothly(position, Vector3.zero, duration, _activeCard.DefaultScaleVector);
        }

        public void PlayOnPlace(float duration)
        {
            MoveOnPlace(_cardDropPlaceImitation.GetPosition(), duration);

            _cardDragAndDropListener.OnCardDrag(_activeCard);
        }

        public bool TryReturnToHand(float returnToHandDuration)
        {
            if (_cardDropPlaceImitation.TrySeatCard(_activeCard) == false)
            {
                _cardDragAndDropListener.OnCardDrop();
                _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);

                return true;
            }

            return false;
        }

        private void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _cardTransform.rotation.eulerAngles;
            Vector3 downWay = position;

            _cardMovement.MoveLinear(downWay, rotation, duration);
        }
    }
}