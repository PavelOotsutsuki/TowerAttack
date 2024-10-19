using System;
using UnityEngine;

namespace Tools.UI.Fadings
{
    [Serializable]
    public class FadablePanelData
    {
        [field: SerializeField] public float MaxAlpha { get; private set; } = 1f;
        [field: SerializeField] public float MinAlpha { get; private set; } = 0f;
        [field: SerializeField] public float FadeOutDuration { get; private set; } = 1.5f;
        [field: SerializeField] public float FadeUpDuration { get; private set; } = 1.5f;
        [field: SerializeField] public float StartAlpha { get; private set; } = 0f;
    }
}