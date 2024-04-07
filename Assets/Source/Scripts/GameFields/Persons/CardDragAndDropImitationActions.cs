using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private Card _activeCard;
        private ICardTransformable _cardTransformable;
        private RectTransform _cardTransform;
        private Movement _cardMovement;

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
            _cardTransformable = card;
            _cardTransform = _cardTransformable.Transform;
            _cardMovement = new Movement(_cardTransform);
        }

        public void ViewCard(float duration, float yDirection)
        {
            Vector3 position = _cardTransform.localPosition;
            position.y += _cardTransform.rect.height / 2 * yDirection;

            _cardMovement.MoveLocalSmoothly(position, Vector3.zero, duration, _cardTransformable.DefaultScaleVector);
        }

        public void PlayOnPlace(float duration)
        {
            MoveOnPlace(_cardDropPlaceImitation.GetCentralСoordinates(), duration);

            _cardDragImitationListener.OnCardDrag(_activeCard);
        }

        public bool TryReturnToHand(float returnToHandDuration)
        {
            if (_cardDropPlaceImitation.TrySeatCard(_activeCard) == false)
            {
                _cardDragImitationListener.OnCardDrop();
                _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _cardTransformable.DefaultScaleVector);

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