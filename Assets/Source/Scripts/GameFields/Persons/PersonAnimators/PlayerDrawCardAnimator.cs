using UnityEngine;
using Cards;
using System;

namespace GameFields.Persons.PersonAnimators
{
    [Serializable]
    internal class PlayerDrawCardAnimator
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        private Transform _parent;

        internal PlayerDrawCardAnimator(Transform parent)
        {
            _parent = parent;
        }

        internal float GetFullDelay()
        {
            return _invertCardBackDuration + _invertCardFrontDuration + _delay;
        }

        internal void StartDrawCardAnimation(Card drawnCard)
        {
            DrawnCardBehaviour(drawnCard);
        }

        private void DrawnCardBehaviour(Card drawnCard)
        {
            drawnCard.transform.SetParent(_parent);
            drawnCard.transform.SetAsLastSibling();
            drawnCard.Drawn(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _invertCardFrontDuration, _indent);
        }
    }
}
