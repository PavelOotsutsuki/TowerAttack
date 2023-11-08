using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cards
{
    public class CardMovement: MonoBehaviour
    {
        private RectTransform _cardRectTransform;
        private ICardProtectable _cardProtectable;

        private Sequence _currentMovement;
        
        public void Init(RectTransform cardRectTransform, ICardProtectable cardProtectable)
        {
            _cardRectTransform = cardRectTransform;
            _cardProtectable = cardProtectable;
        }

        public void Stop()
        {
            _currentMovement?.Kill();
        }
        
        public void TranslateLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f)
        {
            //StartCoroutine(ProtectCard(duration));

            Vector3 scaleVector = new Vector3(scaleX, scaleY, scaleZ);

            _currentMovement = DOTween.Sequence()
                .Join(_cardRectTransform.DOLocalMove(positon, duration))
                .Join(_cardRectTransform.DORotate(rotation, duration))
                .Join(_cardRectTransform.DOScale(scaleVector, duration));

            StartCoroutine(UnblockCard(duration));
        }

        public void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration, float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f)
        {
            //StartCoroutine(ProtectCard(duration));

            Vector3 scaleVector = new Vector3(scaleX, scaleY, scaleZ);

            _cardRectTransform.DOMove(positon, duration);
            _cardRectTransform.DORotate(rotation, duration);
            _cardRectTransform.DOScale(scaleVector, duration);
        }

        public void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f)
        {
            //StartCoroutine(ProtectCard(duration));

            Vector3 scaleVector = new Vector3(scaleX, scaleY, scaleZ);

            _cardRectTransform.DOMove(downWay, duration).SetEase(Ease.Linear);
            _cardRectTransform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
            _cardRectTransform.DOScale(scaleVector, duration).SetEase(Ease.Linear);
        }

        //private IEnumerator ProtectCard(float duration)
        //{
        //    _cardProtectable.Block();

        //    yield return new WaitForSeconds(duration);

        //    _cardProtectable.Unblock();
        //}

        private IEnumerator UnblockCard(float duration)
        {
            yield return new WaitForSeconds(duration);

            _cardProtectable.Unblock();
        }
    }
}
