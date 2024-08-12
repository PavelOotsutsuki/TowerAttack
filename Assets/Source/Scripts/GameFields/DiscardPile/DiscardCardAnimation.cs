using System.Collections;
using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Tools;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimation
    {
        private Card _card;
        private ICardTransformable _cardTransformable;
        private Transform _cardTransform;
        private Movement _cardMovement;

        private DiscardCardAnimationData _data;
        private Transform _container;
        private Action<Card> _callback;

        public DiscardCardAnimation(DiscardCardAnimationData data, Transform container, Card card, Action<Card> callback)
        {
            _data = data;
            _container = container;
            _callback = callback;

            _card = card;
            _cardTransformable = card;
            _cardTransform = _cardTransformable.Transform;
            _cardMovement = new Movement(_cardTransform);
        }

        public void Play()
        {
            DiscardingCard().ToUniTask();
        }

        private IEnumerator DiscardingCard()
        {
            _cardTransform.SetParent(_container);
            
            _card.SetDiscardSide();

            _cardTransformable.SetActiveInteraction(false);
            _cardTransformable.SetSide(SideType.Front);

            IncreaseCard();
            yield return new WaitForSeconds(_data.CardIncreaseDuration + _data.DelayAfterIncrease);

            InvertCardFront();
            yield return new WaitForSeconds(_data.InvertCardFrontDuration);

            _cardTransformable.SetSide(SideType.Back);

            InvertCardBack();
            yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            _callback?.Invoke(_card);
        }

        private void InvertCardFront()
        {
            Vector3 scaleVector = _cardTransformable.DefaultScaleVector;
            Vector3 position = _cardTransform.position;

            _cardMovement.MoveLinear(position, _data.InvertRotation, _data.InvertCardFrontDuration, scaleVector);
        }

        private void InvertCardBack()
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _cardTransform.position;

            _cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _cardTransform.localScale);
        }

        private void IncreaseCard()
        {
            Vector3 startPosition = _card.GetPosition();

            _cardMovement.MoveInstantly(startPosition, _data.StartRotation, _data.StartScaleVector);
            _cardMovement.MoveSmoothly(_cardTransform.position, _cardTransform.rotation.eulerAngles, _data.CardIncreaseDuration, _cardTransformable.DefaultScaleVector);
        }
    }
}
