using System;
using GameFields.CommonAnimations;
using Tools;
using Tools.CommonAnimations;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [Serializable]
    public class CardAttackZoneData: IData
    {
        [field: SerializeField] public AttackAnimationData AttackAnimationData { get; private set; }
        //[field: SerializeField] public InvertCardAnimationData InvertCardAnimationData { get; private set; }
        [field: SerializeField] public ShakeAnimationConfig ShakeAnimationConfig { get; private set; }
        //[field: SerializeField] public float DelayBeforeStartingEndFightActions { get; private set; } = 5f;
        [field: SerializeField] public int NeedSelectForAttack { get; private set; } = 1;
    }
}