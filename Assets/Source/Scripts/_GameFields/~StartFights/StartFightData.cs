using System;
using Tools;
using UnityEngine;

namespace GameFields.StartFights
{
    [Serializable]
    public class StartFightData: IData
    {
        [field: SerializeField] public int FirstTurnCardsCount { get; private set; } = 3;
        [field: SerializeField] public float WaitToStartDuration { get; private set; } = 2f;
        [field: SerializeField] public float DelayToViewEnemySolutionLabel { get; private set; } = 0.5f;
        [field: SerializeField] public float WaitUntilBeginAllProcess { get; private set; } = 1f;
        [field: SerializeField] public float WaitAfterStartEndGamePanelComplete { get; private set; } = 1f;
    }
}