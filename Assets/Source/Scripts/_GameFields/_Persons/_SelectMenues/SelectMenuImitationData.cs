using System;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    [Serializable]
    public class SelectMenuImitationData : SelectMenuData
    {
        [field: SerializeField] public float DelayAfterChoiceNumberDone { get; private set; } = 1f;
    }
}