using System;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class SimpleDrawCardAnimationData
    {
        [field: SerializeField] public float Delay { get; private set; } = 0.1f;
    }
}
