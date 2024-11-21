using UnityEngine;
using System;

namespace GameFields.DiscardPiles
{
    [Serializable]
    public class DiscardCardAnimationData
    {
        [field: SerializeField] public Vector3 StartScaleVector { get; private set; } = new(0.5f, 0.5f, 0.5f);
        [field: SerializeField] public Vector3 StartRotation { get; private set; } = Vector3.zero;
        [field: SerializeField] public Vector3 InvertRotation { get; private set; } = new(0f, -90f, 0f);
        [field: SerializeField] public float CardIncreaseDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayAfterIncrease { get; private set; } = 0.5f;
        [field: SerializeField] public float InvertCardFrontDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float InvertCardBackDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayAfterInvert { get; private set; } = 0.5f;

        public float GetFullDelay()
        {
            return CardIncreaseDuration + DelayAfterIncrease + InvertCardBackDuration + InvertCardFrontDuration + DelayAfterInvert;
        }
    }
}
