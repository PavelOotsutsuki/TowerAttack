using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.Persons.Fires;
using GameFields.Persons.Hands;
using Tools;
using Tools.Settings;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private readonly FireDrawCardAnimationData _data;
        private readonly FirePool _firePool;

        private bool _isComplete;

        public FireDrawCardAnimation(FireDrawCardAnimationData data, FirePool firePool)
        {
            _data = data;
            _firePool = firePool;

            _isComplete = true;
        }

        public bool IsComplete => _isComplete;

        public void Play(Card card)
        {
            Playing(card).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            _isComplete = false;

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
            CallbackHandler callbackHandlerFire = new CallbackHandler();
            drawnCard.Fire(new WaitForSeconds(
                _data.StartMoveDuration +
                _data.InvertCardAnimationData.InvertCardFrontDuration +
                _data.InvertCardAnimationData.InvertCardBackDuration +
                _data.InvertCardAnimationData.DelayAfterInvert +
                _data.FireDrawCardDelay), callbackHandlerFire); // Так, а не потом, потому что надо чтобы sound пироманта пошел сразу

            drawnCard.RORTransform.SetParent(_data.FireDrawTemporarilyParent);

            drawnCard.CardMovement.MoveLocalLinear(_data.EndStartMovePosition, drawnCard.RORTransform.GetRotationVector(),
                _data.StartMoveDuration);

            yield return new WaitForSeconds(_data.StartMoveDuration);

            InvertCardAnimation invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            InvertCardAnimationPlayData playData = _data.InvertCardAnimationPlayData;
            invertCardAnimation.Play(drawnCard, playData);

            //cardMovement.MoveLocalSmoothly(firstPosition, readOnlyRectTransform.GetRotationVector(), 0.5f, scale);
            yield return new WaitUntil(() => invertCardAnimation.IsComplete);
            //yield return new WaitForSeconds(_data.FireDrawCardDelay);
            yield return new WaitUntil(() => callbackHandlerFire.IsComplete);


            //yield return new WaitForSeconds(2.5f);
            drawnCard.gameObject.SetActive(false);

            _firePool.SeatCard(drawnCard);
            _isComplete = true;
        }
    }
}