using System;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    [Serializable]
    public class AttackMenuImitationData : AttackMenuData
    {
        [field: SerializeField] public float DelayAfterChoiceNumberDone { get; private set; } = 1f;
    }
}