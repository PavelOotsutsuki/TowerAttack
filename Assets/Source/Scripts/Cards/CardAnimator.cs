using UnityEngine;

namespace Cards
{
    internal class CardAnimator : MonoBehaviour
    {
        private DrawCardAnimation _drawCardAnimation;

        public void Init(RectTransform cardRectTransform, CardMovement cardMovement, CardSideFlipper sideFlipper)
        {
            _drawCardAnimation = new DrawCardAnimation(cardRectTransform, cardMovement, sideFlipper);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent, float screenFactor)
        {
            StartCoroutine(_drawCardAnimation.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent, screenFactor));
        }
    }
}
