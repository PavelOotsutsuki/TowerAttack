using System;
using System.Collections;
using UnityEngine;

namespace Cards
{
    internal class DrawCardAnimation
    {
        private readonly CardView _cardView;
        private readonly RectTransform _rectTransform;
        private readonly CardMovement _cardMovement;
        
        public DrawCardAnimation(CardView cardView, RectTransform rectTransform, CardMovement cardMovement)
        {
            _cardView = cardView;
            _rectTransform = rectTransform;
            _cardMovement = cardMovement;
        }
       
        public event Action Finish;
        
        internal IEnumerator Play(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            InvertCardBack(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _cardView.SetFrontSide();

            InvertCardFront(cardFrontDuration, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardFrontDuration);

            Finish?.Invoke();
        }

        private void InvertCardBack(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            float scaleX = _rectTransform.localScale.x * cardBackScaleFactor;
            float scaleY = _rectTransform.localScale.y * cardBackScaleFactor;
            float scaleZ = _rectTransform.localScale.z * cardBackScaleFactor;
            
            Vector3 downWay =_rectTransform.position;
            downWay.y -= (downWay.y - indent - _rectTransform.rect.height) / 2;

            _cardMovement.TranslateLinear(downWay, endRotationVector, cardBackDuration, scaleX, scaleY, scaleZ);
        }

        private void InvertCardFront(float duration, float scaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            float scaleX = scaleFactor;
            float scaleY = scaleFactor;
            float scaleZ = scaleFactor;
            
            Vector3 downWay = _rectTransform.position;
            downWay.y = _rectTransform.rect.height / 2 * scaleFactor + indent;

            _cardMovement.TranslateSmoothly(downWay, endRotationVector, duration, scaleX, scaleY, scaleZ);
        }
    }
}