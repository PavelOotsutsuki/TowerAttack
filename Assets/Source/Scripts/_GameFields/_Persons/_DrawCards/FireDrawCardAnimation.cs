using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.Persons.Hands;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private readonly FireDrawCardAnimationData _data;

        private bool _isComplete;

        public FireDrawCardAnimation(FireDrawCardAnimationData data)
        {
            _data = data;

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

            float endScale = 1.5f;
            Vector3 scale = new Vector3(endScale, endScale, endScale);
            float centerScale = endScale - (endScale - 1f) / 2f;
            Vector3 centerScaleVector = new Vector3(centerScale, centerScale, centerScale);

            Vector2 firstPosition = new Vector2(-400f, 200f);

            Movement cardMovement = new Movement(drawnCard.transform);
            ReadOnlyRectTransform readOnlyRectTransform = drawnCard.ReadOnlyRectTransform;

            InvertCardAnimation invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            InvertCardAnimationPlayData playData = new InvertCardAnimationPlayData(firstPosition / 2f, centerScaleVector, firstPosition, scale);
            invertCardAnimation.Play(drawnCard, playData);

            //cardMovement.MoveLocalSmoothly(firstPosition, readOnlyRectTransform.GetRotationVector(), 0.5f, scale);

            yield return new WaitForSeconds(_data.FireDrawCardDelay);

            drawnCard.gameObject.SetActive(false);

            _isComplete = true;
        }
    }
}