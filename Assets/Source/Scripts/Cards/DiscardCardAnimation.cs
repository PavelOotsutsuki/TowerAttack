using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Cards
{
    internal class DiscardCardAnimation
    {
        private CardMovement _cardMovement;
        private ICardSides _cardSides;

        public DiscardCardAnimation(CardMovement cardMovement, ICardSides sides)
        {
            _cardMovement = cardMovement;
            _cardSides = sides;
        }

        public void PlayDiscardCardAnimation(Vector3 startPosition, DiscardCardAnimationData discardCardAnimationData)
        {
            PlayingDiscardCardAnimation(startPosition, discardCardAnimationData).ToUniTask();
        }

        private IEnumerator PlayingDiscardCardAnimation(Vector3 startPosition, DiscardCardAnimationData discardCardAnimationData)
        {
            Vector3 startScaleVector = discardCardAnimationData.StartScaleVector;
            Vector3 startRotation = discardCardAnimationData.StartRotation;
            Vector3 invertRotation = discardCardAnimationData.InvertRotation;
            float cardIncreaseDuration = discardCardAnimationData.CardIncreaseDuration;
            float delayAfterIncrease = discardCardAnimationData.DelayAfterIncrease;
            float invertCardFrontDuration = discardCardAnimationData.InvertCardFrontDuration;
            float invertCardBackDuration = discardCardAnimationData.InvertCardBackDuration;
            float delayAfterInvert = discardCardAnimationData.DelayAfterInvert;

            _cardSides.SetFrontSide();
            
            _cardMovement.IncreaseCard(startPosition, startRotation, startScaleVector, cardIncreaseDuration);
            yield return new WaitForSeconds(cardIncreaseDuration + delayAfterIncrease);

            _cardMovement.InvertCardFrontOnDiscard(invertRotation, invertCardFrontDuration);
            yield return new WaitForSeconds(invertCardFrontDuration);

            _cardSides.SetBackSide();

            _cardMovement.InvertCardBackOnDiscard(invertCardBackDuration);
            yield return new WaitForSeconds(invertCardBackDuration + delayAfterInvert);
        }
    }
}
