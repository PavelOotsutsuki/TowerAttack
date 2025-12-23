using System;
using GameFields.CommonAnimations;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    [Serializable]
    public class AttackResultHandlerData : IData
    {
        [field: SerializeField] public InvertCardAnimationData InvertCardAnimationData { get; private set; }
        [field: SerializeField] public float DelayBeforeStartingEndFightActions { get; private set; } = 5f;
    }
}