using UnityEngine;
using DG.Tweening;

namespace Tools
{
    public class TransformPositionChanger
    {
        private Transform _transfrom;

        public TransformPositionChanger(Transform transform)
        {
            _transfrom = transform;
        }

        public void TranslateLocalInstantly(Vector2 positon, Vector3 rotation)
        {
            _transfrom.Translate(positon);
            _transfrom.Rotate(rotation);
        }

        public void TranslateLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _transfrom.DOLocalMove(positon, duration);
            _transfrom.DOLocalRotate(rotation, duration);
            _transfrom.DOScale(scaleVector, duration);
        }

        public void TranslateLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        {
            _transfrom.DOLocalMove(positon, duration);
            _transfrom.DOLocalRotate(rotation, duration);
        }

        public void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _transfrom.DOMove(positon, duration);
            _transfrom.DORotate(rotation, duration);
            _transfrom.DOScale(scaleVector, duration);
        }

        public void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _transfrom.DOMove(downWay, duration).SetEase(Ease.Linear);
            _transfrom.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
            _transfrom.DOScale(scaleVector, duration).SetEase(Ease.Linear);
        }

        public void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration)
        {
            _transfrom.DOMove(downWay, duration).SetEase(Ease.Linear);
            _transfrom.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
        }
    }
}
