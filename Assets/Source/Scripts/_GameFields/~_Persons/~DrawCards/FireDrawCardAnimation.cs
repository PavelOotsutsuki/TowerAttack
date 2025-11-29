using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
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

        public void Play(IReadOnlyList<Card> cards, int indexAdd)
        {
            Playing(cards, indexAdd).ToUniTask();
        }

        private IEnumerator Playing(IReadOnlyList<Card> cards, int indexAdd)
        {
            _isComplete = false;

            if (cards.Count <= 0)
            {
                _isComplete = true;
                yield break;
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

                Firing(cards[i], i, callbackHandlerFire, indexAdd).ToUniTask();
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitUntil(() => lastCallbackHandlerFire.IsComplete && (lastPyromancersManuscriptCallbackHandlerFire == null || lastPyromancersManuscriptCallbackHandlerFire.IsComplete));
            _isComplete = true;
        }

        private IEnumerator Firing(Card drawnCard, int level, CallbackHandler callbackHandlerSeatInFirePool, int indexAdd)
        {
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
            drawnCard.Fire(new WaitForSeconds(
                _data.StartMoveDuration +
                _data.InvertCardAnimationData.InvertCardFrontDuration +
                _data.InvertCardAnimationData.InvertCardBackDuration +
                _data.InvertCardAnimationData.DelayAfterInvert +
                _data.FireDrawCardDelay), callbackHandlerFire); // Так, а не потом, потому что надо чтобы sound пироманта пошел сразу

            drawnCard.RORTransform.SetParent(_data.FireDrawTemporarilyParent);

            drawnCard.CardMovement.MoveLocalLinear(endStartMovePosition, drawnCard.RORTransform.GetRotationVector(),
                _data.StartMoveDuration);

            yield return new WaitForSeconds(_data.StartMoveDuration);

            InvertCardAnimation invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            invertCardAnimation.Play(drawnCard, playData);

            //cardMovement.MoveLocalSmoothly(firstPosition, readOnlyRectTransform.GetRotationVector(), 0.5f, scale);
            yield return new WaitUntil(() => invertCardAnimation.IsComplete);
            //yield return new WaitForSeconds(_data.FireDrawCardDelay);
            yield return new WaitUntil(() => callbackHandlerFire.IsComplete);


            //yield return new WaitForSeconds(2.5f);
            drawnCard.gameObject.SetActive(false);

            if (indexAdd == -1)
                indexAdd = _viewHand.AllCards.Count();

            _firePool.SeatCard(drawnCard, _seatableHand, indexAdd, callbackHandlerSeatInFirePool);

            yield return new WaitUntil(() => callbackHandlerSeatInFirePool.IsComplete);
        }
    }
}