using System;
using Tools;
using UnityEngine;

namespace GameFields.InformationLabels
{
    [Serializable]
    public class InformationLabelData : IData
    {
        [field: SerializeField] public string DefaultInformationLabelText { get; private set; } = "Противник выбрал номер: ";
        [field: SerializeField] public float TimeViewInformationLabel { get; private set; } = 4f;
    }
}