using UnityEngine;
using DG.Tweening;

namespace Cards
{
    public class CardMovement
    {
        private RectTransform _cardRectTransform;

        private Sequence _currentMovement;

        public CardMovement(RectTransform cardRectTransform)
        {
            _cardRectTransform = cardRectTransform;
        }

        public void TranslateLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOLocalMove(positon, duration))
                .Join(_cardRectTransform.DORotate(rotation, duration))
                .Join(_cardRectTransform.DOScale(scaleVector, duration));
        }

        public void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOMove(positon, duration))
                .Join(_cardRectTransform.DORotate(rotation, duration))
                .Join(_cardRectTransform.DOScale(scaleVector, duration));
        }

        public void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOMove(downWay, duration).SetEase(Ease.Linear))
                .Join(_cardRectTransform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear))
                .Join(_cardRectTransform.DOScale(scaleVector, duration).SetEase(Ease.Linear));
        }
    }
}
