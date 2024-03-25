using UnityEngine;

namespace Cards
{
    internal class CardAnimator
    {
        private DrawCardAnimation _drawCardAnimation;
        private DiscardCardAnimation _discardCardAnimation;
        //private DragAndDropAnimation _dragAndDropAnimation;

        public CardAnimator (ICardSides sides,  CardMovement cardMovement)
        {
            _drawCardAnimation = new DrawCardAnimation(sides, cardMovement);
            _discardCardAnimation = new DiscardCardAnimation(cardMovement, sides);
            //_dragAndDropAnimation = new DragAndDropAnimation(cardRectTransform, cardMovement);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            _drawCardAnimation.PlayPlayerDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent);
        }

        public void PlayDiscardCardAnimation(Vector3 startPosition, DiscardCardAnimationData discardCardAnimationData)
        {
            _discardCardAnimation.PlayDiscardCardAnimation(startPosition, discardCardAnimationData);
        }
    }
}
