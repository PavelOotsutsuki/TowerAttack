using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    internal class DragAndDropAnimation
    {
        private RectTransform _cardRectTransform;
        private CardMovement _cardMovement;

        public DragAndDropAnimation(RectTransform cardRectTransform, CardMovement cardMovement)
        {
            _cardRectTransform = cardRectTransform;
            _cardMovement = cardMovement;
        }

        public void PlaySelectCardAnimation(float screenFactor, float duration)
        {
            Vector3 rotation = _cardRectTransform.rotation.eulerAngles;
            Vector3 scaleVector = _cardRectTransform.localScale;
            Vector3 downWay = _cardRectTransform.position;

            downWay.y += _cardRectTransform.rect.height / 2 * screenFactor;

            _cardMovement.TranslateLocalSmoothly(downWay, rotation, duration, scaleVector);
        }

        public void PlayUnselectCardAnimation(float screenFactor, float duration)
        {
            Vector3 rotation = _cardRectTransform.rotation.eulerAngles;
            Vector3 scaleVector = _cardRectTransform.localScale;
            Vector3 downWay = _cardRectTransform.position;

            downWay.y -= _cardRectTransform.rect.height / 2 * screenFactor;

            _cardMovement.TranslateLocalSmoothly(downWay, rotation, duration, scaleVector);
        }

        public void PlayCardAnimation()
        {

        }
    }
}
