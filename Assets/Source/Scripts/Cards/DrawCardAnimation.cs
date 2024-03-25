using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Cards
{
    internal class DrawCardAnimation
    {
        private readonly ICardSides _sides;
        private readonly CardMovement _cardMovement;

        public DrawCardAnimation(ICardSides sides, CardMovement cardMovement)
        {
            _sides = sides;
            _cardMovement = cardMovement;
        }
        
        public void PlayPlayerDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            PlayingPlayerDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent).ToUniTask();
        }

        private IEnumerator PlayingPlayerDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            _sides.SetFrontSide();
            
            _cardMovement.InvertCardBackOnDraw(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _sides.SetFrontSide();

            _cardMovement.InvertCardFrontOnDraw(cardFrontDuration, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardFrontDuration);
        }
    }
}
