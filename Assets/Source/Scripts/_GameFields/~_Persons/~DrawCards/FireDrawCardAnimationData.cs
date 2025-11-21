using System;
using GameFields.CommonAnimations;
using GameFields.Persons.Fires;
using Tools.Settings;
using Tools.Utils.Screens;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class FireDrawCardAnimationData
    {
        private const int MaxLevel = 20;

        [field: SerializeField] public InvertCardAnimationData InvertCardAnimationData { get; private set; }
        [field: SerializeField] public float FireDrawCardDelay { get; private set; } = 1f;
        [field: SerializeField] public float StartMoveDuration { get; private set; } = 0.3f;
        [SerializeField, Min(1f)] private float _endScale = 2f;
        [SerializeField] private bool _isDirectionUp = true;
        [SerializeField, Range(0f, 0.9f)] private float _startInvertPositionPercent = 0.33f;
        [SerializeField, Range(0f, 0.9f)] private float _centerInvertPositionPercent = 0.66f;
        [SerializeField] private FireDrawTemporarilyContainer _fireDrawTemporarilyContainer;

        private float _offsetX;
        private float _offsetY;
        //private float _maxOffsetX;
        //private float _maxOffsetY;
        private float _offsetStepX;
        private float _offsetStepY;
        private float _minOffsetX;
        private float _minOffsetY;

        public void Init()
        {
            _offsetX = 0f;
            _offsetY = 0f;
            _minOffsetX = GameSettings.CardSize.x * _endScale * (-1f);
            _minOffsetY = GameSettings.CardSize.y * _endScale * (-1f);
            float maxOffsetX = ScreenView.X() / 2f * (-1f);
            float maxOffsetY = GameSettings.CardSize.y * _endScale * (-1f);
            _offsetStepX = (maxOffsetX - _minOffsetX) / MaxLevel;
            _offsetStepY = (maxOffsetY - _minOffsetY) / MaxLevel;
        }

        public void SetNewOffsets(int level)
        {
            if (level > MaxLevel)
                level = MaxLevel;

            if (level < 1)
                level = 1;

            float currentLevelOffsetX = _offsetStepX * level;
            float currentLevelOffsetY = _offsetStepY * level;

            _offsetX = Random.Range(currentLevelOffsetX, currentLevelOffsetX + _minOffsetX);
            _offsetY = Random.Range(currentLevelOffsetY, currentLevelOffsetY + _minOffsetY);
            Debug.Log($"level={level}, currentLevelOffsetX={currentLevelOffsetX}, currentLevelOffsetY={currentLevelOffsetY}," +
                $"_offsetX={_offsetX}, _offsetY={_offsetY}");
        }

        public void ResetOffsets()
        {
            _offsetX = 0f;
            _offsetY = 0f;
        }

        private float Direction => _isDirectionUp == true ? 1 : -1;
        private float CenterScale => _endScale - (_endScale - 1f) / 2f;
        private Vector2 EndPosition => new Vector2((GameSettings.CardSize.x * 2 * (-1f)) + _offsetX, (GameSettings.CardSize.y + _offsetY) * Direction);
        private Vector3 CenterScaleVector => new Vector3(CenterScale, CenterScale, CenterScale);
        private Vector3 EndScaleVector => new Vector3(_endScale, _endScale, _endScale);
        private Vector3 CenterInvertPosition => EndPosition * _centerInvertPositionPercent;

        public Vector3 EndStartMovePosition => EndPosition * _startInvertPositionPercent;
        public InvertCardAnimationPlayData InvertCardAnimationPlayData => new InvertCardAnimationPlayData(CenterInvertPosition, CenterScaleVector, EndPosition, EndScaleVector);
        public Transform FireDrawTemporarilyParent => _fireDrawTemporarilyContainer.GetTransform();
    }
}