using System;
using UnityEngine;

namespace Tools.UI
{
    [Serializable]
    public class NascentData : IData
    {
        [field: SerializeField] public Vector2 StartScale { get; private set; } = Vector2.zero;
        [field: SerializeField] public Vector2 EndScale { get; private set; } = new Vector2(1f, 1f);
        [field: SerializeField] public float Duration { get; private set; } = 0.3f;
    }
}