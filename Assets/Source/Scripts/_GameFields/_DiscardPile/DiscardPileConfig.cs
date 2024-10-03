using System;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    [Serializable]
    public class DiscardPileConfig
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public float CardRotationOffset { get; private set; } = 30f;
        [field: SerializeField] public float StartCardTranslateSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float DiscardDelay { get; private set; } = 0.5f;
        [field: SerializeField] public DiscardCardAnimationData DiscardCardAnimationData { get; private set; }
    }
}
