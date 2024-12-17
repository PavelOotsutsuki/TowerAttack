using Cards;
using Tools;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using System.Collections;
using Tools.Utils.Movements;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private readonly ICardDragAndDropListener _cardDragAndDropListener;
        private readonly ICardDropPlace _cardDropPlaceImitation;
        private readonly SignalBus _bus;

        private Card _activeCard;
        private ReadOnlyRectTransform _readOnlyCardTransform;
        private Movement _cardMovement;

        private bool _isMoving;

        public CardDragAndDropImitationActions(ICardDragAndDropListener cardDragListener, ICardDropPlace cardDropPlaceImitation, SignalBus bus)
        {
            _cardDragAndDropListener = cardDragListener;
            _cardDropPlaceImitation = cardDropPlaceImitation;
            _bus = bus;
            _isMoving = false;
        }

        internal void SetCard(Card card)
        {
            _activeCard = card;
            _readOnlyCardTransform = _activeCard.ReadOnlyRectTransform;
            _cardMovement = _activeCard.CardMovement;
        }

        public void ViewCard(float duration, float yDirection)
        {
            Vector3 position = _readOnlyCardTransform.GetLocalPosition();
            position.y += _readOnlyCardTransform.GetHeight() / 2 * yDirection;

            _cardMovement.MoveLocalSmoothly(position, Vector3.zero, duration, _activeCard.DefaultScaleVector);
        }

        public void MoveOnPlace(float duration)
        {
            _isMoving = true;

            MoveOnPlace(_cardDropPlaceImitation.ReadOnlyRectTransform.GetPosition(), duration);

            _cardDragAndDropListener.OnCardDrag(_activeCard);
        }

        public bool CanPlay() => _cardDropPlaceImitation.HasFreeSeat;

        public IEnumerator Play()
        {
            yield return new WaitUntil(() => _isMoving == false);

            _cardDragAndDropListener.OnCardPlay();
            _cardDropPlaceImitation.SeatCard(_activeCard);
            //_bus.Fire(new StartEffectSignal(_activeCard));
        }

        public void ReturnInHand(float returnToHandDuration)
        {
            _cardDragAndDropListener.OnCardDrop();
            _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);
        }

        private void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _readOnlyCardTransform.GetRotationVector();
            Vector3 downWay = position;

            _cardMovement.MoveLinear(downWay, rotation, duration, () => _isMoving = false);
        }
    }
}