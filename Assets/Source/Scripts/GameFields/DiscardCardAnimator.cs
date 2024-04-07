using System.Collections;
using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Tools;

namespace GameFields
{
    [Serializable]
    public class DiscardCardAnimator
    {
        [SerializeField] private Transform _container;
        //[SerializeField] private DiscardCardAnimationData _discardCardAnimationData;
        [SerializeField] public Vector3 _startScaleVector = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] public Vector3 _startRotation  = Vector3.zero;
        [SerializeField] public Vector3 _invertRotation = new Vector3(0f, -90f, 0f);
        [SerializeField] public float _cardIncreaseDuration  = 0.5f;
        [SerializeField] public float _delayAfterIncrease = 0.5f;
        [SerializeField] public float _invertCardFrontDuration = 0.5f;
        [SerializeField] public float _invertCardBackDuration = 0.5f;
        [SerializeField] public float _delayAfterInvert = 0.5f;

        private Card _card;
        private ICardTransformable _cardTransformable;
        private Transform _cardTransform;
        private Movement _cardMovement;

        public void DiscardCard(Card card)
        {
            _cardTransformable = card;
            _cardTransform = _cardTransformable.Transform;
            _cardMovement = new Movement(_cardTransform);

            DiscardingCard().ToUniTask();
        }

        public float GetFullDelay()
        {
            return _cardIncreaseDuration + _delayAfterIncrease + _invertCardBackDuration + _invertCardFrontDuration + _delayAfterInvert;
        }

        private IEnumerator DiscardingCard()
        {
            _cardTransform.SetParent(_container);
            
            _card.DiscardCard();

            _cardTransformable.SetSide(SideType.DisableFront);

            IncreaseCard();
            yield return new WaitForSeconds(_cardIncreaseDuration + _delayAfterIncrease);

            InvertCardFront();
            yield return new WaitForSeconds(_invertCardFrontDuration);

            _cardTransformable.SetSide(SideType.Back);

            InvertCardBack();
            yield return new WaitForSeconds(_invertCardBackDuration + _delayAfterInvert);
        }

        private void InvertCardFront()
        {
            Vector3 scaleVector = _cardTransformable.DefaultScaleVector;
            Vector3 position = _cardTransform.position;

            _cardMovement.MoveLinear(position, _invertRotation, _invertCardFrontDuration, scaleVector);
        }

        private void InvertCardBack()
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _cardTransform.position;

            _cardMovement.MoveSmoothly(position, endRotationVector, _invertCardBackDuration, _cardTransform.localScale);
        }

        private void IncreaseCard()
        {
            Vector3 startPosition = _card.GetCardCharacterPosition();

            _cardMovement.MoveInstantly(startPosition, _startRotation, _startScaleVector);
            _cardMovement.MoveSmoothly(_cardTransform.position, _cardTransform.rotation.eulerAngles, _cardIncreaseDuration, _cardTransformable.DefaultScaleVector);
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
