using UnityEngine;
using Cards;
using System;
using System.Collections;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    internal class PlayerSimpleDrawCardAnimator
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        internal IEnumerator StartDrawCardAnimation(Card drawnCard, Transform parent)
        {
            drawnCard.transform.SetParent(parent);
            drawnCard.transform.SetAsLastSibling();
            drawnCard.Drawn(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _invertCardFrontDuration, _indent);
            yield return new WaitForSeconds(GetFullDelay());
        }

        private float GetFullDelay()
        {
            return _invertCardBackDuration + _invertCardFrontDuration + _delay;
        }
    }
}
