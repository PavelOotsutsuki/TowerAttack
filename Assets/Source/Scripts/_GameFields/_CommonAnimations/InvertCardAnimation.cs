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

        public InvertCardAnimation(InvertCardAnimationData data)
        {
            IsComplete = false;
            _data = data;
        }

        public bool IsComplete { get; private set; }

        public void Play(Card card)
        {
            IsComplete = false;

            _card = card;
            _cardMovement = _card.CardMovement;
            _readOnlyCardTransform = _card.ReadOnlyRectTransform;

            Playing().ToUniTask();
        }

        private IEnumerator Playing()
        {
            if (_data.IsIgnoreStartSide == true || _card.CurrentSide == SideType.Front)
            {
                InvertCardFront();
                yield return new WaitForSeconds(_data.InvertCardFrontDuration);
            }

            _card.SetSide(SideType.Back);

            InvertCardBack();
            yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            IsComplete = true;
        }

        private void InvertCardFront()
        {
            Vector3 invertRotation = new Vector3(0f, -90f, 0f);
            //Vector3 scaleVector = _card.DefaultScaleVector;
            Vector3 position = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveLinear(position, invertRotation, _data.InvertCardFrontDuration, _readOnlyCardTransform.GetLocalScale());
        }

        private void InvertCardBack()
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = _readOnlyCardTransform.GetPosition();

            _cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, _readOnlyCardTransform.GetLocalScale());
        }
    }
}
