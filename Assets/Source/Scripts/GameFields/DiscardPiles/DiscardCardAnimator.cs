using System.Collections;
using Cards;
using UnityEngine;
using System;

namespace GameFields
{
    [Serializable]
    public class DiscardCardAnimator
    {
        [SerializeField] private Transform _container;
        [SerializeField] private DiscardCardAnimationData _discardCardAnimationData;

        public IEnumerator DiscardCards(Card card)
        {
            card.transform.SetParent(_container);
            card.DiscardCard(_discardCardAnimationData);

            yield return new WaitForSeconds(GetFullDelay());
        }

        public float GetFullDelay()
        {
            return _discardCardAnimationData.GetFullDelay();
        }
    }
}
