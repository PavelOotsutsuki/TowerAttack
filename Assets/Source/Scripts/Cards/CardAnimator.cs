using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Cards
{
    internal class CardAnimator
    {
        private DrawCardAnimation _drawCardAnimation;
        //private DragAndDropAnimation _dragAndDropAnimation;

        public CardAnimator (RectTransform cardRectTransform, CardMovement cardMovement, CardSideFlipper sideFlipper)
        {
            _drawCardAnimation = new DrawCardAnimation(cardRectTransform, cardMovement, sideFlipper);
            //_dragAndDropAnimation = new DragAndDropAnimation(cardRectTransform, cardMovement);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            _drawCardAnimation.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent).ToUniTask();
        }

        //public void PlaySelectCardAnimation(float screenFactor, float duration)
        //{
        //    _dragAndDropAnimation.PlaySelectCardAnimation(screenFactor, duration);
        //}

        //public void PlayUnselectCardAnimation(float screenFactor, float duration)
        //{
        //    _dragAndDropAnimation.PlayUnselectCardAnimation(screenFactor, duration);
        //}

        //public void PlayCardAnimation(Vector3 center, float duration)
        //{
        //    _dragAndDropAnimation.PlayCardAnimation(center, duration);
        //}

        //public void PlayReturnInHandAnimation(float duration)
        //{
        //    _dragAndDropAnimation.PlayReturnInHandAnimation(duration);
        //}
    }
}
