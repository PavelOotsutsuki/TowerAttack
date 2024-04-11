using System.Collections;
using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Tools;

namespace GameFields.DiscardPiles
{
    //[Serializable]
    public class DiscardCardAnimation
    {
        //[SerializeField] private Transform _container;
        ////[SerializeField] private DiscardCardAnimationData _discardCardAnimationData;
        //[SerializeField] public Vector3 _startScaleVector = new Vector3(0.5f, 0.5f, 0.5f);
        //[SerializeField] public Vector3 _startRotation  = Vector3.zero;
        //[SerializeField] public Vector3 _invertRotation = new Vector3(0f, -90f, 0f);
        //[SerializeField] public float _cardIncreaseDuration  = 0.5f;
        //[SerializeField] public float _delayAfterIncrease = 0.5f;
        //[SerializeField] public float _invertCardFrontDuration = 0.5f;
        //[SerializeField] public float _invertCardBackDuration = 0.5f;
        //[SerializeField] public float _delayAfterInvert = 0.5f;

        private Card _card;
        private ICardTransformable _cardTransformable;
        private Transform _cardTransform;
        private Movement _cardMovement;

        private DiscardCardAnimationData _data;
        private Transform _container;
        private Func<Card, bool> _returnToHandCallback;

        public DiscardCardAnimation(DiscardCardAnimationData data, Transform container, Card card, Func<Card, bool> returnToHandCallback)
        {
            _data = data;
            _container = container;
            _returnToHandCallback = returnToHandCallback;

            _card = card;
            _cardTransformable = card;
            _cardTransform = _cardTransformable.Transform;
            _cardMovement = new Movement(_cardTransform);

            DiscardingCard().ToUniTask();
        }

        //public void DiscardCard(Card card)
        //{
        //    _card = card;
        //    _cardTransformable = card;
        //    _cardTransform = _cardTransformable.Transform;
        //    _cardMovement = new Movement(_cardTransform);

        //    DiscardingCard().ToUniTask();
        //}

        //public float GetFullDelay()
        //{
        //    return _cardIncreaseDuration + _delayAfterIncrease + _invertCardBackDuration + _invertCardFrontDuration + _delayAfterInvert;
        //}

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

            _returnToHandCallback?.Invoke(_card);
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
