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

        public void MoveLocalInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            _transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
            _transform.localScale = scaleVector;
        }

        public void MoveInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            _transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
            _transform.localScale = scaleVector;
        }

        public void MoveInstantly(Vector2 position, Vector3 rotation)
        {
            _transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        }

        public void MoveLocalSmoothly(Vector2 position, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                MoveLocalInstantly(position, rotation, scaleVector);
            }
            else
            {
                Sequence sequence = DOTween.Sequence()
                .Join(_transform.DOLocalMove(position, duration))
                .Join(_transform.DOLocalRotate(rotation, duration))
                .Join(_transform.DOScale(scaleVector, duration));

                _currentSequence = sequence;
            }
        }

        public void MoveLocalSmoothly(Vector2 position, Vector3 rotation, float duration)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                MoveLocalInstantly(position, rotation);
            }
            else
            {
                Sequence sequence = DOTween.Sequence()
                .Join(_transform.DOLocalMove(position, duration))
                .Join(_transform.DOLocalRotate(rotation, duration));

                _currentSequence = sequence;
            }
        }

        public void MoveSmoothly(Vector2 position, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                MoveInstantly(position, rotation, scaleVector);
            }
            else
            {
                Sequence sequence = DOTween.Sequence()
                .Join(_transform.DOMove(position, duration))
                .Join(_transform.DORotate(rotation, duration))
                .Join(_transform.DOScale(scaleVector, duration));

                _currentSequence = sequence;
            }
        }

        //public void MoveSmoothly(Vector2 position, Vector3 rotation, float duration)
        //{
        //    Sequence sequence = DOTween.Sequence()
        //    .Join(_transform.DOMove(position, duration))
        //    .Join(_transform.DORotate(rotation, duration));

        //    _currentSequence = sequence;
        //}

        public void MoveLinear(Vector3 position, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                MoveInstantly(position, rotation, scaleVector);
            }
            else
            {
                Sequence sequence = DOTween.Sequence()
                .Join(_transform.DOMove(position, duration).SetEase(Ease.Linear))
                .Join(_transform.DORotate(rotation, duration).SetEase(Ease.Linear))
                .Join(_transform.DOScale(scaleVector, duration).SetEase(Ease.Linear));

                _currentSequence = sequence;
            }
        }

        public void MoveLinear(Vector3 position, Vector3 rotation, float duration)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                MoveInstantly(position, rotation);
            }
            else
            {
                Sequence sequence = DOTween.Sequence()
                .Join(_transform.DOMove(position, duration).SetEase(Ease.Linear))
                .Join(_transform.DORotate(rotation, duration).SetEase(Ease.Linear));

                _currentSequence = sequence;
            }
        }
    }
}
