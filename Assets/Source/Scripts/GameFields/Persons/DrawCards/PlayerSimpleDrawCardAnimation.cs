using UnityEngine;
using Cards;
using System;
using System.Collections;
using Tools;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    internal class PlayerSimpleDrawCardAnimation
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        private RectTransform _cardTransform;
        private Movement _movement;

        internal IEnumerator StartAnimation(ICardTransformable drawnCard, Transform container)
        {
            _cardTransform = drawnCard.Transform;
            _movement = new Movement(_cardTransform);

            drawnCard.Transform.SetParent(container);
            drawnCard.Transform.SetAsLastSibling();

            InvertCardBack(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _indent);
            yield return new WaitForSeconds(_invertCardBackDuration);

            drawnCard.SetActiveInteraction(false);
            drawnCard.SetSide(SideType.Front);

            InvertCardFrontOnDraw(_invertCardFrontDuration, _invertCardBackScaleFactor, _indent);
            yield return new WaitForSeconds(_invertCardFrontDuration + _delay);
        }

        private void InvertCardBack(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            Vector3 scaleVector = _cardTransform.localScale * cardBackScaleFactor;

            Vector3 downWay = _cardTransform.position;
            downWay.y -= (downWay.y - indent - _cardTransform.rect.height) / 2;

            _movement.MoveLinear(downWay, endRotationVector, cardBackDuration, scaleVector);
        }

        private void InvertCardFrontOnDraw(float duration, float scaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            Vector3 downWay = _cardTransform.position;
            float screenfactor = ScreenView.GetFactorY();

            downWay.y = (_cardTransform.rect.height / 2f * scaleFactor + indent) * screenfactor;

            _movement.MoveSmoothly(downWay, endRotationVector, duration, _cardTransform.localScale);
        }
    }
}
