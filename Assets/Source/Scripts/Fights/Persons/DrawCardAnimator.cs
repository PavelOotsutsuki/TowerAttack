using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cards;
using Hands;

namespace Fights.Persons
{
    internal class DrawCardAnimator : MonoBehaviour
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        internal void Init(Hand hand, Card drawnCard, CanvasScaler canvasScaler)
        {
            StartCoroutine(DrawnCardBehaviour(hand, drawnCard, canvasScaler));
        }

        private IEnumerator DrawnCardBehaviour(Hand hand, Card drawnCard, CanvasScaler canvasScaler)
        {
            float fullDelay = _invertCardBackDuration + _invertCardFrontDuration + _delay;
            float screenFactor = Screen.height / canvasScaler.referenceResolution.y;

            drawnCard.transform.SetParent(transform);
            drawnCard.transform.SetAsLastSibling();
            drawnCard.PlayDrawnCardAnimation(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _invertCardFrontDuration, _indent, screenFactor);

            yield return new WaitForSeconds(fullDelay);

            hand.AddCard(drawnCard);
        }
    }
}
