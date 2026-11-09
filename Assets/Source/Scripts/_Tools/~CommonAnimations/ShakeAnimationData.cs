using System;
using UnityEngine;

namespace Tools.CommonAnimations
{
    [Serializable]
    public class ShakeAnimationData : IData
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.2f;
        [field: SerializeField] public float MaxOffsetX { get; private set; } = 0.3f;
        [field: SerializeField] public float MaxOffsetY { get; private set; } = 0.3f;
        [field: SerializeField] public float MinOffsetX { get; private set; } = -0.3f;
        [field: SerializeField] public float MinOffsetY { get; private set; } = -0.3f;
        [field: SerializeField] public float Delay { get; private set; } = 0.025f;
    }
}