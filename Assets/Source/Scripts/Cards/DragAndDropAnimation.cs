using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
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
            Vector3 rotation = _cardRectTransform.localRotation.eulerAngles;
            Vector3 downWay = _cardRectTransform.localPosition;

            downWay.y += _cardRectTransform.rect.height / 2 * screenFactor;

            _cardMovement.MoveLocalSmoothly(downWay, rotation, duration);
        }

        public void PlayUnselectCardAnimation(float screenFactor, float duration)
        {
            Vector3 rotation = _cardRectTransform.localRotation.eulerAngles;
            Vector3 downWay = _cardRectTransform.localPosition;

            downWay.y -= _cardRectTransform.rect.height / 2 * screenFactor;

            _cardMovement.MoveLocalSmoothly(downWay, rotation, duration);
        }

        public void PlayCardAnimation(Vector3 center, float duration)
        {
            Vector3 rotation = _cardRectTransform.rotation.eulerAngles;
            Vector3 downWay = center;

            _cardMovement.MoveLinear(downWay, rotation, duration);
        }

        public void PlayReturnInHandAnimation(float duration)
        {
            Vector3 scaleVector = new Vector3(1f, 1f, 1f);
            Vector3 localPosition = new Vector3(0f, 0f, 0f);
            Vector3 rotation = Vector3.zero;

            _cardMovement.MoveLocalSmoothly(localPosition, rotation, duration, scaleVector);
        }
    }
}
