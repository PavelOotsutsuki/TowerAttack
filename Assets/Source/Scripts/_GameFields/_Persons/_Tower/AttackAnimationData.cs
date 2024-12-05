using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [Serializable]
    public class AttackAnimationData: IData
    {
        [field: SerializeField] public float OffsetXCardFactor { get; } = 0.35f;
        [field: SerializeField] public float OffsetYCardFactor { get; } = 0.35f;
    }
}
