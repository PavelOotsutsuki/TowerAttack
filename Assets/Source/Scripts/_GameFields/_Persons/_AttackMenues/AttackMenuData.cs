using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [Serializable]
    public abstract class AttackMenuData : IData
    {
        [field: SerializeField] public bool IsInteractable { get; private set; } 
        [field: SerializeField] public string AttackMenuLabelText { get; private set; }
    }
}