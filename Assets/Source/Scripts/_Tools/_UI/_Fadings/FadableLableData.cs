using System;
using UnityEngine;

namespace Tools.UI.Fadings
{
    [Serializable]
    public class FadableLableData: IData
    {
        [field: SerializeField] public string StartText { get; private set; } = "";
    }
}