using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardMovement
    {
        private Vector3 _defaultScaleVector;
        private RectTransform _cardTransform;
        private Movement _movement;

        public CardMovement(RectTransform transform, Vector3 defaultScaleVector)
        {
            _defaultScaleVector = defaultScaleVector;
            _cardTransform = transform;
            _movement = new Movement(_cardTransform);
        }

        public void InvertCardFrontOnDiscard(Vector3 rotation, float duration)
        {
            Vector3 scaleVector = _defaultScaleVector;
            Vector3 position = _cardTransform.position;

            _movement.MoveLinear(position, rotation, duration, scaleVector);
        }

        public void InvertCardBackOnDiscard(float duration)
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _cardTransform.position;

            _movement.MoveSmoothly(position, endRotationVector, duration, _cardTransform.localScale);
        }

        public void IncreaseCard(Vector3 startPosition, Vector3 startRotation, Vector3 startScaleVector, float cardIncreaseDuration)
        {
            _movement.MoveInstantly(startPosition, startRotation, startScaleVector);
            _movement.MoveLocalSmoothly(_cardTransform.position, _cardTransform.rotation.eulerAngles, cardIncreaseDuration, _defaultScaleVector);
        }

        public void MoveReturnToHand(float duration)
        {
            _movement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, duration, _defaultScaleVector);
        }

        public void ViewCardMovement(ViewType viewType, float duration)
        {
            Vector3 position = _cardTransform.localPosition;

            switch (viewType)
            {
                case ViewType.Select:
                    position.y += _cardTransform.rect.height / 2;
                    break;
                case ViewType.Unselect:
                    position.y -= _cardTransform.rect.height / 2;
                    break;
                default:
                    Debug.LogError("Unknown ViewType");
                    break;
            }

            _movement.MoveLocalSmoothly(position, Vector3.zero, duration, _defaultScaleVector);
        }

        public void BindSeatMovement(float duration)
        {
            _movement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _defaultScaleVector);
        }

        public void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _cardTransform.rotation.eulerAngles;
            Vector3 downWay = position;

            _movement.MoveLinear(downWay, rotation, duration);
        }

        public void InvertCardBackOnDraw(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            Vector3 scaleVector = _cardTransform.localScale * cardBackScaleFactor;

            Vector3 downWay = _cardTransform.position;
            downWay.y -= (downWay.y - indent - _cardTransform.rect.height) / 2;

            _movement.MoveLinear(downWay, endRotationVector, cardBackDuration, scaleVector);
        }

        public void InvertCardFrontOnDraw(float duration, float scaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            Vector3 downWay = _cardTransform.position;
            float screenfactor = ScreenView.GetFactorY();

            downWay.y = (_cardTransform.rect.height / 2f * scaleFactor + indent) * screenfactor;

            _movement.MoveSmoothly(downWay, endRotationVector, duration, _cardTransform.localScale);
        }
    }
}
