using System.Collections;
using UnityEngine;
using Cards;
using Hands;

namespace Persons
{
    internal class DrawCardAnimator : MonoBehaviour
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        internal void Init(Hand hand, Card drawnCard)
        {
            StartCoroutine(DrawnCardBehaviour(hand, drawnCard));
        }

        private IEnumerator DrawnCardBehaviour(Hand hand, Card drawnCard)
        {
            float fullDelay = _invertCardBackDuration + _invertCardFrontDuration + _delay;

            drawnCard.transform.SetParent(transform);
            drawnCard.transform.SetAsLastSibling();
            drawnCard.PlayDrawnCardAnimation(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _invertCardFrontDuration, _indent);

            yield return new WaitForSeconds(fullDelay);

            hand.AddCard(drawnCard);
        }
    }
}
