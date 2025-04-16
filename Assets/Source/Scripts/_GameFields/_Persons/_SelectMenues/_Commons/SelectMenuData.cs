using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    [Serializable]
    public abstract class SelectMenuData : IData
    {
        [field: SerializeField] public string SelectMenuLabelText { get; private set; }
    }
}