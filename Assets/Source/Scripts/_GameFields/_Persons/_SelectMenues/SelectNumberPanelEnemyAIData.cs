using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    [Serializable]
    public class SelectNumberPanelEnemyAIData : IData
    {
        [field: SerializeField] public float DelayThinkImitation { get; private set; } = 8f;
        [field: SerializeField] public string DefaultInformationLabelText { get; private set; } = "Противник выбрал номер: ";
        [field: SerializeField] public float TimeViewInformationLabel { get; private set; } = 4f;
    }
}