using System;
using UnityEngine;

namespace GameFields.EndFights
{
    [Serializable]
    public class EndFightConfig
    {
        [field: SerializeField] public EndFightLabelActivateData PlayerWinData { get; private set; }
        [field: SerializeField] public EndFightLabelActivateData EnemyWinData { get; private set; }
        [field: SerializeField] public EndFightLabelActivateData DrawData { get; private set; }
        [field: SerializeField] public float DelayBeforeEndFightLabelShow { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayBeforeExitFightLabelShow { get; private set; } = 2f;
        [field: SerializeField] public float DelayBeforeAddedExperienceLabelShow { get; private set; } = 1f;
    }
}