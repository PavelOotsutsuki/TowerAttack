using System.Collections;
using UnityEngine;

namespace Cards
{
    internal class DrawCardAnimation
    {
        private RectTransform _cardRectTransform;
        private CardView _cardView;
        private CardMovement _cardMovement;
        private ICardProtectable _cardProtectable;

        public DrawCardAnimation(RectTransform cardRectTransform, CardView cardView, CardMovement cardMovement, ICardProtectable cardProtectable)
        {
            _cardRectTransform = cardRectTransform;
            _cardView = cardView;
            _cardMovement = cardMovement;
            _cardProtectable = cardProtectable;
        }

        public IEnumerator PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            //_dragAndDropable.ActivateDragAndDrop(false);
            //_cardFront.Block();

            InvertCardBack(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _cardView.SetFrontSide();

            InvertCardFront(cardFrontDuration, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardFrontDuration);

            _cardProtectable.Unblock();
            //_dragAndDropable.ActivateDragAndDrop(true);
            //_cardFront.Unblock();
        }

        private void InvertCardBack(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            Vector3 scaleVector = _cardRectTransform.localScale * cardBackScaleFactor;

            Vector3 downWay = _cardRectTransform.position;
            downWay.y -= (downWay.y - indent - _cardRectTransform.rect.height) / 2;

            _cardMovement.TranslateLinear(downWay, endRotationVector, cardBackDuration, scaleVector);
        }

        private void InvertCardFront(float duration, float scaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            Vector3 downWay = _cardRectTransform.position;
            downWay.y = _cardRectTransform.rect.height / 2 * scaleFactor + indent; 

            _cardMovement.TranslateSmoothly(downWay, endRotationVector, duration, _cardRectTransform.localScale);
        }

    }
}
