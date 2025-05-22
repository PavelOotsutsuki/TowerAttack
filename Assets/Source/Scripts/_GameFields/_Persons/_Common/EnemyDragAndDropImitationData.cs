using System;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Common
{
    [Serializable]
    public class EnemyDragAndDropImitationData : IData
    {
        [field: SerializeField] public float StartDelayMin { get; private set; } = 1f;
        [field: SerializeField] public float StartDelayMax { get; private set; } = 2f;
        [field: SerializeField] public float CardViewTime { get; private set; } = 1f;
        [field: SerializeField] public float CardViewDelayMin { get; private set; } = 2f;
        [field: SerializeField] public float CardViewDelayMax { get; private set; } = 4f;
        [field: SerializeField] public float CardTranslateInDropPlaceTime { get; private set; } = 0.5f;
        [field: SerializeField] public float CardReturnInHandTime { get; private set; } = 0.5f;
        [field: SerializeField] public float EndTurnDelay { get; private set; } = 0.5f;
        [field: SerializeField] public int MaxCountRepeat { get; private set; } = 2;
        [field: SerializeField] public int CountDrawCards { get; private set; } = 1;
        [field: SerializeField] public float DrawCardsDelay { get; private set; } = 0.5f;
    }
}