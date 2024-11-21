using System;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    [Serializable]
    public class DiscardPileConfig
    {
        [field: SerializeField] public RectTransform RectTransform;
        [field: SerializeField] public float CardRotationOffset = 30f;
        [field: SerializeField] public float StartCardTranslateSpeed = 0.5f;
        [field: SerializeField] public float DiscardDelay = 0.5f;
        [field: SerializeField] public DiscardCardAnimationData DiscardCardAnimationData;
    }
}
