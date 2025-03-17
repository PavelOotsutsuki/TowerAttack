using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.CommonAnimations
{
    public class InvertCardAnimation : ICompletable
    {
        private readonly InvertCardAnimationData _data;

        private Card _card;
        private Movement _cardMovement;
        private ReadOnlyRectTransform _readOnlyCardTransform;

        private InvertCardAnimationPlayData _playData;

        public InvertCardAnimation(InvertCardAnimationData data)
        {
            IsComplete = false;
            _data = data;
        }

        public bool IsComplete { get; private set; }

        public void Play(Card card, InvertCardAnimationPlayData data = null)
        {
            IsComplete = false;

            _card = card;
            _cardMovement = _card.CardMovement;
            _readOnlyCardTransform = _card.ReadOnlyRectTransform;

            _playData = data ?? new InvertCardAnimationPlayData(_readOnlyCardTransform.GetLocalPosition(), _readOnlyCardTransform.GetLocalScale(), _readOnlyCardTransform.GetLocalPosition(), _readOnlyCardTransform.GetLocalScale());

            Playing().ToUniTask();
        }

        private IEnumerator Playing()
        {
            if (_data.IsIgnoreStartSide == true || _card.CurrentSide == _data.StartSide)
            {
                InvertCardStartSide();
                yield return new WaitForSeconds(_data.InvertCardFrontDuration);
            }

            _card.SetSide(_data.FinishSide);

            InvertCardFinishSide();
            yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            IsComplete = true;
        }

        private void InvertCardStartSide()
        {
            Vector3 invertRotation = new Vector3(0f, -90f, 0f);
            //Vector3 scaleVector = _card.DefaultScaleVector;
            Vector3 position = _playData.StartSidePosition;

            //_cardMovement.MoveLinear(position, invertRotation, _data.InvertCardFrontDuration, _playData.StartSideScale);
            _cardMovement.MoveLocalLinear(position, invertRotation, _data.InvertCardFrontDuration, _playData.StartSideScale);
        }

        private void InvertCardFinishSide()
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _playData.FinishSidePosition;

            //_cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _playData.FinishSideScale);
            _cardMovement.MoveLocalSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _playData.FinishSideScale);
        }
    }
}
