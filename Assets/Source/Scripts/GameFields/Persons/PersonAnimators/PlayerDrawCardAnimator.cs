using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cards;
using GameFields.Persons.Hands;

namespace GameFields.Persons.PersonAnimators
{
    internal class PlayerDrawCardAnimator : PersonDrawCardAnimator
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        private Hand _hand;
        private CanvasScaler _canvasScaler;

        internal void Init(Hand hand, CanvasScaler canvasScaler)
        {
            _hand = hand;
            _canvasScaler = canvasScaler;
        }

        internal void StartDrawCardAnimation(Card drawnCard)
        {
            StartCoroutine(DrawnCardBehaviour(drawnCard));
        }

        private IEnumerator DrawnCardBehaviour(Card drawnCard)
        {
            float fullDelay = _invertCardBackDuration + _invertCardFrontDuration + _delay;
            float screenFactor = Screen.height / _canvasScaler.referenceResolution.y;

            drawnCard.transform.SetParent(transform);
            drawnCard.transform.SetAsLastSibling();
            drawnCard.PlayDrawnCardAnimation(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _invertCardFrontDuration, _indent, screenFactor);

            yield return new WaitForSeconds(fullDelay);

            _hand.AddCard(drawnCard);
        }
    }
}
