using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Cards
{
    internal class DiscardCardAnimation
    {
        private RectTransform _rectTransform;
        private CardMovement _cardMovement;
        private CardSideFlipper _cardSideFlipper;

        public DiscardCardAnimation(RectTransform rectTransform, CardMovement cardMovement, CardSideFlipper sideFlipper)
        {
            _rectTransform = rectTransform;
            _cardMovement = cardMovement;
            _cardSideFlipper = sideFlipper;
        }

        public IEnumerator PlayDiscardCardAnimation(Vector3 startPosition, DiscardCardAnimationData discardCardAnimationData)
        {
            Vector3 startScaleVector = discardCardAnimationData.StartScaleVector;
            Vector3 startRotation = discardCardAnimationData.StartRotation;
            Vector3 invertRotation = discardCardAnimationData.InvertRotation;
            float cardIncreaseDuration = discardCardAnimationData.CardIncreaseDuration;
            float delayAfterIncrease = discardCardAnimationData.DelayAfterIncrease;
            float invertCardFrontDuration = discardCardAnimationData.InvertCardFrontDuration;
            float invertCardBackDuration = discardCardAnimationData.InvertCardBackDuration;
            float delayAfterInvert = discardCardAnimationData.DelayAfterInvert;

            _cardSideFlipper.SetFrontSide();
            _cardSideFlipper.Block();

            _cardMovement.IncreaseCard(startPosition, startRotation, startScaleVector, cardIncreaseDuration);
            yield return new WaitForSeconds(cardIncreaseDuration + delayAfterIncrease);

            _cardMovement.InvertCardFrontOnDiscard(invertRotation, invertCardFrontDuration);
            yield return new WaitForSeconds(invertCardFrontDuration);

            _cardSideFlipper.SetBackSide();

            _cardMovement.InvertCardBackOnDiscard(invertCardBackDuration);
            yield return new WaitForSeconds(invertCardBackDuration + delayAfterInvert);
        }
    }
}
