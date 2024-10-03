using System;
using UnityEngine;

namespace GameFields.StartFights
{
    [Serializable]
    public class StartTowerCardSelectionPlayerData
    {
        [field: SerializeField] public float WaitDurationForEnemyFirstCardsDraw { get; private set; } = 1f;
        [field: SerializeField] public float DrawCardsDuration { get; private set; } = 1.5f;
        [field: SerializeField] public float DrawCardsScaleFactor { get; private set; } = 2f;
        [field: SerializeField] public float WaitDurationBetweenDrawCards { get; private set; } = 1f;
        [field: SerializeField] public string LabelMessage { get; private set; } = "Выберете, какая карта будет в замке";
        [field: SerializeField] public float DelayAfterCardChoiceDone { get; private set; } = 0.5f;
        [field: SerializeField] public float InvertCardFrontDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float InvertCardBackDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayAfterInvert { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayBeforeStartProcessSeatCardInTower { get; private set; } = 1f;
    }
}