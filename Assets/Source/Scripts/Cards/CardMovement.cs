using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cards
{
    public class CardMovement
    {
        private RectTransform _cardRectTransform;
        //private ICardProtectable _cardProtectable;

        private Sequence _currentMovement;

        public CardMovement(RectTransform cardRectTransform)
        {
            _cardRectTransform = cardRectTransform;
            //_cardProtectable = cardProtectable;
        }

        //public void Stop()
        //{
        //    Debug.Log("Kill");
        //    _currentMovement?.Kill();
        //}

        public void TranslateLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            //StartCoroutine(ProtectCard(duration));

            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOLocalMove(positon, duration))
                .Join(_cardRectTransform.DORotate(rotation, duration))
                .Join(_cardRectTransform.DOScale(scaleVector, duration));

            //StartCoroutine(UnblockCard(duration));
        }

        public void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            //StartCoroutine(ProtectCard(duration));
            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOMove(positon, duration))
                .Join(_cardRectTransform.DORotate(rotation, duration))
                .Join(_cardRectTransform.DOScale(scaleVector, duration));
        }

        //public void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    //StartCoroutine(ProtectCard(duration));
        //    _currentMovement = DOTween.Sequence()
        //        .Join(_cardRectTransform.DOMove(positon, duration))
        //        .Join(_cardRectTransform.DORotate(rotation, duration));
        //}

        public void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            //StartCoroutine(ProtectCard(duration));
            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOMove(downWay, duration).SetEase(Ease.Linear))
                .Join(_cardRectTransform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear))
                .Join(_cardRectTransform.DOScale(scaleVector, duration).SetEase(Ease.Linear));

        }

        //private IEnumerator ProtectCard(float duration)
        //{
        //    _cardProtectable.Block();

        //    yield return new WaitForSeconds(duration);

        //    _cardProtectable.Unblock();
        //}

        //private IEnumerator UnblockCard(float duration)
        //{
        //    yield return new WaitForSeconds(duration);

        //    _cardProtectable.Unblock();
        //}
    }
}
