using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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

        public IEnumerable PlayDiscardCardAnimation(Vector3 finishPosition, Vector3 finishRotation, Vector3 startPosition)
        {
            yield return ViewCardAfterActivate(startPosition);
        }

        private IEnumerable ViewCardAfterActivate(Vector3 startPosition)
        {
            Vector3 startScaleVector = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 startRotation = Vector3.zero;
            float cardIncreaseDuration = 0.5f;
            float delayAfterIncrease = 0.5f;
            float invertCardFrontDuration = 0.5f;
            float invertCardBackDuration = 0.5f;

            _cardSideFlipper.Block();

            _cardMovement.IncreaseCard(startPosition, startRotation, startScaleVector, cardIncreaseDuration);
            yield return new WaitForSeconds(cardIncreaseDuration + delayAfterIncrease);

            _cardMovement.InvertCardFrontOnDiscard(invertCardFrontDuration);
            yield return new WaitForSeconds(invertCardFrontDuration);

            _cardMovement.InvertCardBackOnDiscard(invertCardBackDuration);
            yield return new WaitForSeconds(invertCardBackDuration);
        }
    }
}
