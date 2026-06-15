using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.Movements;
using GameFields.CommonAnimations;
using Cards.Views;
using System.Threading;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimation
    {
        private readonly Card _card;
        private readonly ReadOnlyTransform _readOnlyCardTransform;
        private readonly Movement _cardMovement;

        private readonly InvertCardAnimation _invertCardAnimation;

        private readonly DiscardCardAnimationData _data;
        private readonly Transform _container;
        private readonly Action<Card> _callback;

        public DiscardCardAnimation(DiscardCardAnimationData data, Transform container, Card card, Action<Card> callback)
        {
            _data = data;
            _container = container;
            _callback = callback;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);

            _card = card;
            _readOnlyCardTransform = _card.RORTransform;
            _cardMovement = _card.CardMovement;
        }

        public void Play(CancellationToken token)
        {
            DiscardingCard(token).Forget();
        }

        private async UniTask DiscardingCard(CancellationToken token)
        {
            _readOnlyCardTransform.SetParent(_container);
            
            _card.SetDiscardSide();

            _card.SetActiveInteraction(false);
            _card.SetSide(SideType.Front);

            IncreaseCard();
            await UniTask.WaitForSeconds(_data.CardIncreaseDuration + _data.DelayAfterIncrease, cancellationToken: token);

            _invertCardAnimation.Play(_card, token);

            //InvertCardFront();
            //yield return new WaitForSeconds(_data.InvertCardFrontDuration);

            //_card.SetSide(SideType.Back);

            //InvertCardBack();
            //yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: token);

            _callback?.Invoke(_card);
        }

        //private void InvertCardFront()
        //{
        //    Vector3 scaleVector = _card.DefaultScaleVector;
        //    Vector3 position = _readOnlyCardTransform.GetPosition();

        //    //_cardMovement.MoveLinear(position, _data.InvertRotation, _data.InvertCardFrontDuration, scaleVector);
        //    _cardMovement.MoveLinear(position, new Vector3(0f, -45f, 0f), _data.InvertCardFrontDuration, scaleVector);
        //}

        //private void InvertCardBack()
        //{
        //    Vector3 endRotationVector = Vector3.zero;
        //    Vector3 position = _readOnlyCardTransform.GetPosition();

        //    _cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _readOnlyCardTransform.GetLocalScale());
        //}

        private void IncreaseCard()
        {
            Vector3 startPosition = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveInstantly(startPosition, _data.StartRotation, _data.StartScaleVector);
            _cardMovement.MoveSmoothly(_readOnlyCardTransform.GetPosition(), _readOnlyCardTransform.GetRotationVector(), _data.CardIncreaseDuration, _card.DefaultScaleVector);
        }
    }
}