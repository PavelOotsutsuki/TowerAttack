using UnityEngine;
using DG.Tweening;
using Tools;

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

        public void MoveReturnToHand(float duration)
        {
            _movement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, duration, _defaultScaleVector);
        }

        public void ViewCardMovement(ViewType viewType, float duration)
        {
            Vector3 position = _cardTransform.localPosition;
            float screenFactor = ScreenView.GetFactorY();
            position.y += _cardTransform.rect.height / 2 * screenFactor;

            _movement.MoveLocalSmoothly(position, Vector3.zero, duration, _defaultScaleVector);
        }

        public void BindSeatMovement(float duration)
        {
            _movement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _defaultScaleVector);
        }

        //public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        //{
        //    _movement.MoveLocalSmoothly(positon, rotation, duration, scaleVector);
        //}

        //public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    _movement.MoveLocalSmoothly(positon, rotation, duration);
        //}

        public void MoveSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _movement.MoveSmoothly(positon, rotation, duration, scaleVector);
        }

        public void MoveLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _movement.MoveLinear(downWay, maxRotationVector, duration, scaleVector);
        }

        //public void MoveLinear(Vector3 downWay, Vector3 maxRotationVector, float duration)
        //{
        //    _movement.MoveLinear(downWay, maxRotationVector, duration);
        //}
    }
}
