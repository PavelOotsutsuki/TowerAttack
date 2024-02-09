using UnityEngine;
using DG.Tweening;

namespace Tools
{
    public class Movement
    {
        private Transform _transform;

        public Movement(Transform transform)
        {
            _transform = transform;
        }

        //public void TranslateLocalInstantly(Vector2 positon, Vector3 rotation)
        //{
        //    _transfrom.Translate(positon);
        //    _transfrom.Rotate(rotation);
        //}
        public void MoveInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            //_transform.position = position;
            //_transform.rotation = Quaternion.Euler(rotation);
            _transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
            _transform.localScale = scaleVector;
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _transform.DOLocalMove(positon, duration);
            _transform.DOLocalRotate(rotation, duration);
            _transform.DOScale(scaleVector, duration);
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        {
            _transform.DOLocalMove(positon, duration);
            _transform.DOLocalRotate(rotation, duration);
        }

        public void MoveSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _transform.DOMove(positon, duration);
            _transform.DORotate(rotation, duration);
            _transform.DOScale(scaleVector, duration);
        }

        public void MoveLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _transform.DOMove(downWay, duration).SetEase(Ease.Linear);
            _transform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
            _transform.DOScale(scaleVector, duration).SetEase(Ease.Linear);
        }

        public void MoveLinear(Vector3 downWay, Vector3 maxRotationVector, float duration)
        {
            _transform.DOMove(downWay, duration).SetEase(Ease.Linear);
            _transform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
        }
    }
}
