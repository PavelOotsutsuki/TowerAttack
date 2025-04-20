using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    [Serializable]
    public class AttackNumberPanelEnemyAIData : IData
    {
        [field: SerializeField] public float DelayThinkImitation { get; private set; } = 8f;
        [field: SerializeField] public string DefaultInformationLabelText { get; private set; } = "Противник выбрал номер: ";
        [field: SerializeField] public float TimeViewInformationLabel { get; private set; } = 4f;
    }
}