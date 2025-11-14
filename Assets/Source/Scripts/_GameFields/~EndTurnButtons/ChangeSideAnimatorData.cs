using System;
using Tools;
using UnityEngine;

namespace GameFields.EndTurnButtons
{
    [Serializable]
    public class ChangeSideAnimatorData: IData
    {
        [field: SerializeField] public float ActiveSideRotation { get; private set; } = 90f;
        [field: SerializeField] public float DeactiveSideRotation { get; private set; } = 0f;
        [field: SerializeField] public GameObject ActiveView { get; private set; }
        [field: SerializeField] public GameObject DeactiveView { get; private set; }
        [field: SerializeField] public RectTransform ButtonTransform { get; private set; }
        [field: SerializeField] public float ActiveViewInvertDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float DeactiveViewInvertDuration { get; private set; } = 0.2f;
    }
}