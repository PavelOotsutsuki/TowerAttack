using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class FadableLableData
    {
        [field: SerializeField] public string StartText { get; private set; } = "";
    }
}
