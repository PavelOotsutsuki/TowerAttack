using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    [Serializable]
    public class SelectNumberPanelPlayerData : IData
    {
        [field: SerializeField] public float NumberWidht { get; private set; } = 100f;
        [field: SerializeField] public float NumberHeight { get; private set; } = 100f;
        [field: SerializeField] public float Indent { get; private set; } = 50f;
        [field: SerializeField, Range(0.1f, 1f)] public float NextAnimationStartPercent { get; private set; } = 0.8f;
        [field: SerializeField] public float DelayAfterAllNumbersAnimationsPlayed { get; private set; } = 1f;
    }
}