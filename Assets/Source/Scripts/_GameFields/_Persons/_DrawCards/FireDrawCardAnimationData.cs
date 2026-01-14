using System;
using GameFields.CommonAnimations;
using GameFields.Persons.Fires;
using Tools.Settings;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class FireDrawCardAnimationData
    {
        [field: SerializeField] public InvertCardAnimationData InvertCardAnimationData { get; private set; }
        [field: SerializeField] public float FireDrawCardDelay { get; private set; } = 1f;
        [field: SerializeField] public float StartMoveDuration { get; private set; } = 0.3f;
        [SerializeField, Min(1f)] private float _endScale = 2f;
        [SerializeField] private bool _isDirectionUp = true;
        [SerializeField, Range(0f, 0.9f)] private float _startInvertPositionPercent = 0.33f;
        [SerializeField, Range(0f, 0.9f)] private float _centerInvertPositionPercent = 0.66f;
        [SerializeField] private FireDrawTemporarilyContainer _fireDrawTemporarilyContainer;

        private float Direction => _isDirectionUp == true ? 1 : -1;
        private float CenterScale => _endScale - (_endScale - 1f) / 2f;
        private Vector2 EndPosition => new Vector2(GameSettings.CardSize.x * 2 * -1, GameSettings.CardSize.y * Direction);
        private Vector3 CenterScaleVector => new Vector3(CenterScale, CenterScale, CenterScale);
        private Vector3 EndScaleVector => new Vector3(_endScale, _endScale, _endScale);
        private Vector3 CenterInvertPosition => EndPosition * _centerInvertPositionPercent;

        public Vector3 EndStartMovePosition => EndPosition * _startInvertPositionPercent;
        public InvertCardAnimationPlayData InvertCardAnimationPlayData => new InvertCardAnimationPlayData(CenterInvertPosition, CenterScaleVector, EndPosition, EndScaleVector);
        public Transform FireDrawTemporarilyParent => _fireDrawTemporarilyContainer.GetTransform();
    }
}