using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    [Serializable]
    public class SelectMenuImitationData : IData
    {
        [field: SerializeField] public float DelayAfterChoiceNumberDone { get; private set; } = 1f;
        [field: SerializeField] public string SelectMenuLabelText { get; private set; } = "Ожидаем противника...";
    }
}