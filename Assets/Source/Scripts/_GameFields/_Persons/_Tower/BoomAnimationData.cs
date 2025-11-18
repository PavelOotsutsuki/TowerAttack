using System;
using Tools;
using Tools.CommonAnimations;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [Serializable]
    public class BoomAnimationData: IData
    {
        [field: SerializeField] public float Duration { get; private set; } = 2f;
        [field: SerializeField] public Color NewCardColor { get; private set; } = Color.red;
        [field: SerializeField] public Color TowerImageColor { get; private set; } = Color.black;
        [field: SerializeField] public float DelayAfterBoomCard { get; private set; } = 0.2f;
        [field: SerializeField] public float DurationAshesDisappear { get; private set; } = 1f;
        [field: SerializeField] public float DelayBetweenStonesBoom { get; private set; } = 0.01f;
        [field: SerializeField] public ShakeAnimationConfig ShakeAnimationConfig { get; private set; }
    }
}