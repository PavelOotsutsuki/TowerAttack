using Cards;
using Tools;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private Card _activeCard;
        private RectTransform _cardTransform;
        private CardMovement _cardMovement;

        private ICardDragAndDropListener _cardDragAndDropListener;
        private ICardDropPlace _cardDropPlaceImitation;
        private SignalBus _bus;
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
            _cardTransform = _activeCard.Transform;
            _cardMovement = _activeCard.CardMovement;
        }

        public void ViewCard(float duration, float yDirection)
        {
            Vector3 position = _cardTransform.localPosition;
            position.y += _cardTransform.rect.height / 2 * yDirection;

            _cardMovement.MoveLocalSmoothly(position, Vector3.zero, duration, _activeCard.DefaultScaleVector);
        }

        public void MoveOnPlace(float duration)
        {
            _isMoving = true;

            MoveOnPlace(_cardDropPlaceImitation.GetPosition(), duration);

            _cardDragAndDropListener.OnCardDrag(_activeCard);
        }

        //public bool TryPlay(float returnToHandDuration)
        //{
        //    if (_cardDropPlaceImitation.HasFreeSeat)
        //    {
        //        Playing().ToUniTask();
        //        return true;
        //    }

        //    _cardDragAndDropListener.OnCardDrop();
        //    _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);

        //    return false;
        //}

        public bool CanPlay()
        {
            return _cardDropPlaceImitation.HasFreeSeat;
        }

        public IEnumerator Play()
        {
            yield return new WaitUntil(() => _isMoving == false);

            _cardDragAndDropListener.OnCardPlay();
            _cardDropPlaceImitation.SeatCard(_activeCard);
            _bus.Fire(new StartEffectSignal(_activeCard));
        }

        public void ReturnInhand(float returnToHandDuration)
        {
            _cardDragAndDropListener.OnCardDrop();
            _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);
        }

        //private IEnumerator Playing()
        //{
        //    yield return new WaitUntil(() => _isMoving == false);

        //    _cardDragAndDropListener.OnCardPlay();
        //    _cardDropPlaceImitation.SeatCard(_activeCard);
        //    _bus.Fire(new StartEffectSignal(_activeCard));
        //}

        private void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _cardTransform.rotation.eulerAngles;
            Vector3 downWay = position;

            _cardMovement.MoveLinear(downWay, rotation, duration, () => _isMoving = false);
        }
    }
}