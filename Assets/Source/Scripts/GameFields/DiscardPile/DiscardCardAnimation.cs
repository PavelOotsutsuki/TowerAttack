using System.Collections;
using Cards;
using UnityEngine;
using System;
using Tools;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimation
    {
        private readonly Card _card;
        private readonly ICardTransformable _cardTransformable;
        private readonly Transform _cardTransform;
        private readonly Movement _cardMovement;

        private readonly DiscardCardAnimationData _data;
        private readonly Transform _container;
        private readonly MonoBehaviour _coroutineContainer;
        private readonly Action<Card> _callback;

        public DiscardCardAnimation(DiscardCardAnimationData data, Transform container, MonoBehaviour coroutineContainer, Card card, Action<Card> callback)
        {
            _data = data;
            _container = container;
            _coroutineContainer = coroutineContainer;
            _callback = callback;

            _card = card;
            _cardTransformable = card;
            _cardTransform = _cardTransformable.Transform;
            _cardMovement = new Movement(_cardTransform);
        }

        public void Play()
        {
            _coroutineContainer.StartCoroutine(DiscardingCard());
        }

        private IEnumerator DiscardingCard()
        {
            _cardTransform.SetParent(_container);
            
            _card.DiscardCard();

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
            Vector3 startPosition = _card.GetCardCharacterPosition();

            _cardMovement.MoveInstantly(startPosition, _data.StartRotation, _data.StartScaleVector);
            _cardMovement.MoveSmoothly(_cardTransform.position, _cardTransform.rotation.eulerAngles, _data.CardIncreaseDuration, _cardTransformable.DefaultScaleVector);
        }

        //private IEnumerator PlayingDiscardCardAnimation()
        //{
        //    _cardSideFlipper.SetFrontSide();
        //    _cardSideFlipper.Block();

        //    _cardMovement.IncreaseCard(startPosition, startRotation, startScaleVector, cardIncreaseDuration);
        //    yield return new WaitForSeconds(cardIncreaseDuration + delayAfterIncrease);

        //    _cardMovement.InvertCardFrontOnDiscard(invertRotation, invertCardFrontDuration);
        //    yield return new WaitForSeconds(invertCardFrontDuration);

        //    _cardSideFlipper.SetBackSide();

        //    _cardMovement.InvertCardBackOnDiscard(invertCardBackDuration);
        //    yield return new WaitForSeconds(invertCardBackDuration + delayAfterInvert);
        //}
    }
}
