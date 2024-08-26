using UnityEngine;
using DG.Tweening;

namespace Tools
{
    public class Movement
    {
        private Transform _transform;
        private Sequence _currentSequence;

        public Movement(Transform transform)
        {
            _transform = transform;
        }

        public void MoveLocalInstantly(Vector2 position, Vector3 rotation)
        {
            _transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
        }

        public void MoveInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            _transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
            _transform.localScale = scaleVector;
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            Sequence sequence = DOTween.Sequence()
            .Join(_transform.DOLocalMove(positon, duration))
            .Join(_transform.DOLocalRotate(rotation, duration))
            .Join(_transform.DOScale(scaleVector, duration));

            _currentSequence = sequence;
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        {
            Sequence sequence = DOTween.Sequence()
            .Join(_transform.DOLocalMove(positon, duration))
            .Join(_transform.DOLocalRotate(rotation, duration));

            _currentSequence = sequence;
        }

        public void MoveSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            Sequence sequence = DOTween.Sequence()
            .Join(_transform.DOMove(positon, duration))
            .Join(_transform.DORotate(rotation, duration))
            .Join(_transform.DOScale(scaleVector, duration));

            _currentSequence = sequence;
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            Sequence sequence = DOTween.Sequence()
            .Join(_transform.DOMove(position, duration).SetEase(Ease.Linear))
            .Join(_transform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear))
            .Join(_transform.DOScale(scaleVector, duration).SetEase(Ease.Linear));

            _currentSequence = sequence;
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration)
        {
            Sequence sequence = DOTween.Sequence()
            .Join(_transform.DOMove(position, duration).SetEase(Ease.Linear))
            .Join(_transform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear));

            _currentSequence = sequence;
        }
    }
}
