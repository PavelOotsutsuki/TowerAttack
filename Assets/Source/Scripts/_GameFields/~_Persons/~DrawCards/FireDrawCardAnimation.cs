using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cards;
using Cards.Animations.Fires;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.CommonAnimations;
using GameFields.Persons.Fires;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private readonly FireDrawCardAnimationData _data;
        private readonly FirePool _firePool;
        private readonly ICardSeatable _seatableHand;
        private readonly ICardView _viewHand;

        private bool _isComplete;

        public FireDrawCardAnimation(FireDrawCardAnimationData data, FirePool firePool, Hand hand)
        {
            _data = data;
            _firePool = firePool;
            _seatableHand = hand;
            _viewHand = hand;

            _data.Init();

            _isComplete = true;
        }

        public bool IsComplete => _isComplete;

        public void Play(IReadOnlyList<Card> cards, int indexAdd, CancellationToken token)
        {
            Playing(cards, indexAdd, token).Forget();
        }

        private async UniTask Playing(IReadOnlyList<Card> cards, int indexAdd, CancellationToken token)
        {
            _isComplete = false;

            if (cards.Count <= 0)
            {
                _isComplete = true;
                return;
            }
            ////
            //float endScale = 1.5f;
            //bool isDirectionUp = true;
            //float startInvertPositionPercent = 0.33f;
            //float centerInvertPositionPercent = 0.66f;

            //float Direction = isDirectionUp == true ? 1 : -1;
            //float CenterScale = endScale - (endScale - 1f) / 2f;
            //Vector2 EndPosition = new Vector2(Settings.CardSize.x * 2 * -1, Settings.CardSize.y * Direction);
            //Vector3 CenterScaleVector = new Vector3(CenterScale, CenterScale, CenterScale);
            //Vector3 EndScaleVector = new Vector3(endScale, endScale, endScale);
            //Vector3 EndStartMovePosition = EndPosition * startInvertPositionPercent;
            //Vector3 CenterInvertPosition = EndPosition * centerInvertPositionPercent;
            ////
            //float startMoveDuration = 0.3f;
            ////

            //float endScale = 1.5f;
            //Vector3 scale = new Vector3(endScale, endScale, endScale);
            //float centerScale = endScale - (endScale - 1f) / 2f;
            //Vector3 centerScaleVector = new Vector3(CenterScale, CenterScale, CenterScale);

            //Vector2 firstPosition = new Vector2(-400f, 200f);
            CallbackHandler lastCallbackHandlerFire = null;
            CallbackHandler lastPyromancersManuscriptCallbackHandlerFire = null;

            for (int i = 0; i < cards.Count; i++)
            {
                CallbackHandler callbackHandlerFire = new CallbackHandler();
                lastCallbackHandlerFire = callbackHandlerFire;

                if (cards[i].IsPyromancersManuscript)
                {
                    lastPyromancersManuscriptCallbackHandlerFire = callbackHandlerFire;
                }

                Firing(cards[i], i, callbackHandlerFire, indexAdd).Forget();
                await UniTask.WaitForSeconds(0.2f, cancellationToken: token);
            }

            await UniTask.WaitUntil(() => lastCallbackHandlerFire.IsComplete && (lastPyromancersManuscriptCallbackHandlerFire == null || lastPyromancersManuscriptCallbackHandlerFire.IsComplete), cancellationToken: token);
            _isComplete = true;
        }

        private async UniTask Firing(Card drawnCard, int level, CallbackHandler callbackHandlerSeatInFirePool, int indexAdd)
        {
            CancellationToken token = drawnCard.CardToken; // Все действия происходят с карты, поэтому берем токен карты

            if (level > 0)
            {
                _data.SetNewOffsets(level);
            }
            else
            {
                _data.ResetOffsets();
            }

            CallbackHandler callbackHandlerFire = new CallbackHandler();
            Vector3 endStartMovePosition = _data.EndStartMovePosition;
            InvertCardAnimationPlayData playData = _data.InvertCardAnimationPlayData;

            //CallbackHandler callbackHandlerFire = new CallbackHandler();
            drawnCard.Fire(
                _data.StartMoveDuration +
                _data.InvertCardAnimationData.InvertCardFrontDuration +
                _data.InvertCardAnimationData.InvertCardBackDuration +
                _data.InvertCardAnimationData.DelayAfterInvert +
                _data.FireDrawCardDelay, callbackHandlerFire); // Так, а не потом, потому что надо чтобы sound пироманта пошел сразу

            drawnCard.RORTransform.SetParent(_data.FireDrawTemporarilyParent);

            drawnCard.CardMovement.MoveLocalLinear(endStartMovePosition, drawnCard.RORTransform.GetRotationVector(),
                _data.StartMoveDuration);

            await UniTask.WaitForSeconds(_data.StartMoveDuration, cancellationToken: token);

            InvertCardAnimation invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            invertCardAnimation.Play(drawnCard, token, playData);

            //cardMovement.MoveLocalSmoothly(firstPosition, readOnlyRectTransform.GetRotationVector(), 0.5f, scale);
            await UniTask.WaitUntil(() => invertCardAnimation.IsComplete, cancellationToken: token);
            //yield return new WaitForSeconds(_data.FireDrawCardDelay);
            await UniTask.WaitUntil(() => callbackHandlerFire.IsComplete, cancellationToken: token);

            //yield return new WaitForSeconds(2.5f);
            drawnCard.gameObject.SetActive(false);

            if (indexAdd == -1)
                indexAdd = _viewHand.AllCards.Count();

            _firePool.SeatCard(drawnCard, _seatableHand, indexAdd, callbackHandlerSeatInFirePool);

            await UniTask.WaitUntil(() => callbackHandlerSeatInFirePool.IsComplete, cancellationToken: token); // Для чего ты ждешь??????
        }
    }
}