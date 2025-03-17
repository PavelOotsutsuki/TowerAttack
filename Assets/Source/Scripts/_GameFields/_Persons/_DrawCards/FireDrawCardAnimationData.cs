using System;
using System.Collections;
using System.Collections.Generic;
using GameFields.CommonAnimations;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class FireDrawCardAnimationData
    {
        [field: SerializeField] public InvertCardAnimationData InvertCardAnimationData { get; private set; }
        [field: SerializeField] public float FireDrawCardDelay { get; private set; } = 2f;
    }
}
