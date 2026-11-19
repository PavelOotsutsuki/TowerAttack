using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [Serializable]
    public class AttackAnimationData: IData
    {
        [field: SerializeField] public float OffsetXCardFactor { get; private set; } = 0.35f;
        [field: SerializeField] public float OffsetYCardFactor { get; private set; } = 0.35f;
        [field: SerializeField] public float AtTheReadyMoveDuration { get; private set; } = 1f;
        [field: SerializeField] public float AfterAtTheReadyMoveDelay { get; private set; } = 0.5f;
        [field: SerializeField] public float EndMoveDuration { get; private set; } = 1f;
        [field: SerializeField] public float InOutBackFactor { get; private set; } = 0.5f;
        [field: SerializeField] public bool IsDown { get; private set; } = true;
    }
}