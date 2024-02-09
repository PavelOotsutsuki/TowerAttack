using System.Collections;
using Tools;
using UnityEngine;

namespace Cards
{
    internal class DrawCardAnimation
    {
        private CardMovement _cardMovement;
        private CardSideFlipper _sideFlipper;

        public DrawCardAnimation(CardMovement cardMovement, CardSideFlipper sideFlipper)
        {
            _sideFlipper = sideFlipper;
            _cardMovement = cardMovement;
        }

        public IEnumerator PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            _cardMovement.InvertCardBackOnDraw(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _sideFlipper.SetFrontSide();
            _sideFlipper.Block();

            _cardMovement.InvertCardFrontOnDraw(cardFrontDuration, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardFrontDuration);
        }
    }
}
