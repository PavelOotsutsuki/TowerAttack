using System.Collections;
using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.Movements;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimation
    {
        private Card _card;
        private ReadOnlyTransform _readOnlyCardTransform;
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
            _readOnlyCardTransform = _card.ReadOnlyRectTransform;
            _cardMovement = _card.CardMovement;
        }

        public void Play()
        {
            DiscardingCard().ToUniTask();
        }

        private IEnumerator DiscardingCard()
        {
            _readOnlyCardTransform.SetParent(_container);
            
            _card.SetDiscardSide();

            _card.SetActiveInteraction(false);
            _card.SetSide(SideType.Front);

            IncreaseCard();
            yield return new WaitForSeconds(_data.CardIncreaseDuration + _data.DelayAfterIncrease);

            InvertCardFront();
            yield return new WaitForSeconds(_data.InvertCardFrontDuration);

            _card.SetSide(SideType.Back);

            InvertCardBack();
            yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            _callback?.Invoke(_card);
        }

        private void InvertCardFront()
        {
            Vector3 scaleVector = _card.DefaultScaleVector;
            Vector3 position = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveLinear(position, _data.InvertRotation, _data.InvertCardFrontDuration, scaleVector);
        }

        private void InvertCardBack()
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _readOnlyCardTransform.GetLocalScale());
        }

        private void IncreaseCard()
        {
            Vector3 startPosition = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveInstantly(startPosition, _data.StartRotation, _data.StartScaleVector);
            _cardMovement.MoveSmoothly(_readOnlyCardTransform.GetPosition(), _readOnlyCardTransform.GetRotationVector(), _data.CardIncreaseDuration, _card.DefaultScaleVector);
        }
    }
}
