using System;
using UnityEngine;

namespace Tools.CommonAnimations
{
    [Serializable]
    public class ShakeAnimationConfig: IData
    {
        [field: SerializeField] public ShakeAnimationData Data { get; private set; }
        [field: SerializeField] public Transform TargetTransform { get; private set; }
    }
}