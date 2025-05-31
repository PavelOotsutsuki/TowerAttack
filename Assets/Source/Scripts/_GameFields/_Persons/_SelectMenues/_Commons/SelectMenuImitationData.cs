using System;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    [Serializable]
    public class SelectMenuImitationData : SelectMenuData
    {
        [field: SerializeField] public float DelayAfterChoiceNumberDone { get; private set; } = 1f;
    }
}