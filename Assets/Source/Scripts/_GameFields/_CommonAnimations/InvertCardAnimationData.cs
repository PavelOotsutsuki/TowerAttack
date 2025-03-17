using System;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.CommonAnimations
{
    [Serializable]
    public class InvertCardAnimationData : IData
    {
        //private readonly float _invertCardFrontDuration;
        //private readonly float _invertCardBackDuration;
        //private readonly float _delayAfterInvert;
        //private readonly Vector3 _invertRotation;

        //public InvertCardAnimationData(float invertCardFrontDuration, float invertCardBackDuration, float delayAfterInvert
        //    , Vector3 invertRotation)
        //{
        //    _invertCardFrontDuration = invertCardFrontDuration;
        //    _invertCardBackDuration = invertCardBackDuration;
        //    _delayAfterInvert = delayAfterInvert;
        //    _invertRotation = invertRotation;
        //}

        //public float InvertCardFrontDuration => _invertCardFrontDuration;
        //public float InvertCardBackDuration => _invertCardBackDuration;
        //public float DelayAfterInvert => _delayAfterInvert;
        //public Vector3 InvertRotation => _invertRotation;
        [field: SerializeField] public float InvertCardFrontDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float InvertCardBackDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayAfterInvert { get; private set; } = 0.5f;
        [field: SerializeField] public bool IsIgnoreStartSide { get; private set; } = false;
        [field: SerializeField] public SideType StartSide { get; private set; } = SideType.Front;

        public SideType FinishSide => StartSide == SideType.Front ? SideType.Back : SideType.Front;
    }
}