using System.Collections;
using UnityEngine;

namespace Cards
{
    internal class DrawCardAnimation
    {
        private RectTransform _cardRectTransform;
        private CardMovement _cardMovement;
        private CardSideFlipper _sideFlipper;

        public DrawCardAnimation(RectTransform cardRectTransform, CardMovement cardMovement, CardSideFlipper sideFlipper)
        {
            _cardRectTransform = cardRectTransform;
            _sideFlipper = sideFlipper;
            _cardMovement = cardMovement;
        }

        public IEnumerator PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent, float screenFactor)
        {
            InvertCardBack(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _sideFlipper.SetFrontSide();

            InvertCardFront(cardFrontDuration, cardBackScaleFactor, indent, screenFactor);
            yield return new WaitForSeconds(cardFrontDuration);
        }

        private void InvertCardBack(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            Vector3 scaleVector = _cardRectTransform.localScale * cardBackScaleFactor;

            Vector3 downWay = _cardRectTransform.position;
            downWay.y -= (downWay.y - indent - _cardRectTransform.rect.height) / 2;

            _cardMovement.TranslateLinear(downWay, endRotationVector, cardBackDuration, scaleVector);
        }

        private void InvertCardFront(float duration, float scaleFactor, float indent, float screenfactor)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            Vector3 downWay = _cardRectTransform.position;
            downWay.y = (_cardRectTransform.rect.height / 2f * scaleFactor + indent) * screenfactor; 

            _cardMovement.TranslateSmoothly(downWay, endRotationVector, duration, _cardRectTransform.localScale);
        }

    }
}
