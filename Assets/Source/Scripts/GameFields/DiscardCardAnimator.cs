using System.Collections;
using Cards;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

namespace GameFields
{
    [Serializable]
    public class DiscardCardAnimator
    {
        [SerializeField] private Transform _container;
        [SerializeField] private DiscardCardAnimationData _discardCardAnimationData;

        public void DiscardCard(Card card)
        {
            DiscardingCard(card).ToUniTask();
        }

        public float GetFullDelay()
        {
            return _discardCardAnimationData.GetFullDelay();
        }

        private IEnumerator DiscardingCard(Card card)
        {
            card.transform.SetParent(_container);
            card.DiscardCard(_discardCardAnimationData);

            yield return new WaitForSeconds(GetFullDelay());
        }
    }
}
