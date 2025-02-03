using Cards;
using Tools;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using System.Collections;
using Tools.Utils.Movements;
using GameFields.Persons.Hands;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons
{
    public class CardDragAndDropImitationActions
    {
        private readonly ICardDragAndDropHandHandler _hand;
        private readonly ICardDropPlace _cardDropPlaceImitation;
        private readonly SignalBus _bus;
        private readonly IAttackable _attackZone;

        private Card _activeCard;
        private ReadOnlyRectTransform _readOnlyCardTransform;
        private Movement _cardMovement;

        private bool _isMoving;

        public CardDragAndDropImitationActions(ICardDragAndDropHandHandler hand, ICardDropPlace cardDropPlaceImitation, SignalBus bus//)
            , IAttackable attackZone)//, ICardDropPlace tower)
        {
            _hand = hand;
            _cardDropPlaceImitation = cardDropPlaceImitation;
            _bus = bus;
            _attackZone = attackZone;

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

            _hand.OnCardDrag(_activeCard);
        }

        public bool CanPlay() => _cardDropPlaceImitation.HasFreeSeat;

        public IEnumerator Play()
        {
            yield return new WaitUntil(() => _isMoving == false);

            _hand.OnCardPlay();
            _cardDropPlaceImitation.SeatCard(_activeCard);
            //_bus.Fire(new StartEffectSignal(_activeCard));
        }

        public void Attack()
        {
            _hand.OnCardDrag(_activeCard);
            //_hand.OnCardAttack();
            _attackZone.Attack(_activeCard);
        }

        public void ReturnInHand(float returnToHandDuration)
        {
            ReturningInHand(returnToHandDuration).ToUniTask();
        }

        private IEnumerator ReturningInHand(float returnToHandDuration)
        {
            yield return new WaitUntil(() => _isMoving == false);

            _hand.OnCardDrop();
            _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);
        }

        private void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _readOnlyCardTransform.GetRotationVector();
            Vector3 downWay = position;

            _cardMovement.MoveLinear(downWay, rotation, duration, () => _isMoving = false);
        }

        ////
        ///
        //public void MoveOnAttackPlace(float duration)
        //{
        //    _isMoving = true;

        //    MoveOnPlace(_cardDropPlaceImitation.ReadOnlyRectTransform.GetPosition(), duration);

        //    _hand.OnCardDrag(_activeCard);
        //}
    }
}