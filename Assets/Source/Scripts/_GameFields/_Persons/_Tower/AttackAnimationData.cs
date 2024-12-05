using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [Serializable]
    public class AttackAnimationData: IData
    {
        [field: SerializeField] public float OffsetXCardFactor { get; } = 0.35f;
        [field: SerializeField] public float OffsetYCardFactor { get; } = 0.35f;
        [field: SerializeField] public float AtTheReadyMoveDuration { get; } = 1f;
        [field: SerializeField] public float AfterAtTheReadyMoveDelay { get; } = 0.5f;
        [field: SerializeField] public float EndMoveDuration { get; } = 1f;
        [field: SerializeField] public float InOutBackFactor { get; } = 0.6f;
    }
}