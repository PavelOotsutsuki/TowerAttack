using System;
using UnityEngine;

namespace Tools.UI.Fadings
{
    [Serializable]
    public class FadableLableData
    {
        [field: SerializeField] public string StartText { get; private set; } = "";
    }
}