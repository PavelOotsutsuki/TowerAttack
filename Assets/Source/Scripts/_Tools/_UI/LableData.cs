using System;
using UnityEngine;

namespace Tools.UI
{
    [Serializable]
    public class LableData: IData
    {
        [field: SerializeField] public string StartText { get; private set; } = "";
    }
}