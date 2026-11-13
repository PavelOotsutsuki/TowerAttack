using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    public class StartLabel : MonoBehaviour, ICompletable, IActivatable, IAutomaticFillComponents
    {
        private const float LifeAlpha = 1f;
        private const float EndAlpha = 0f;
        private const float EndScale = 1f;

        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _startFontSize = 0f;
        [SerializeField] private float _middleFontSize = 120f;
        [SerializeField] private float _endFontSize = 150f;
        [SerializeField] private float _middleDuration = 1f;
        [SerializeField] private float _endDuration = 1f;

        private float _startScale;
        private float _scaleWay;
        private Transform _targetTransform;

        private CancellationToken _fightToken;
        private CancellationTokenSource _currentCTS;

        private Color _startColor;

        public bool IsComplete { get; private set; }

        public void Init(CancellationToken fightToken)
        {
            IsComplete = false;
            gameObject.SetActive(false);

            _targetTransform = _label.transform;
            _fightToken = fightToken;

            _startColor = new Color(_label.color.r, _label.color.g, _label.color.b, LifeAlpha);
            _startScale = _startFontSize / _middleFontSize;
            _scaleWay = ((_middleFontSize / _endFontSize) - _startScale) / _middleDuration;

            _label.color = _startColor;
            //_label.fontSize = _startFontSize;
            _label.fontSize = _endFontSize;
        }

        public void Activate()
        {
            IsComplete = false;
            _label.color = _startColor;
            //_label.fontSize = _startFontSize;
            _label.fontSize = _endFontSize;

            Utils.Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Activating(_currentCTS.Token).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            gameObject.SetActive(true);

            //float startScale = _startFontSize / _middleFontSize;
            //float startFontSize = _label.fontSize;
            //float fontSizeWay = (_middleFontSize - startFontSize) / _middleDuration;

            float startScale = _startScale;
            float scaleWay = _scaleWay;

            for (float time = 0f; time < _middleDuration; time += Time.deltaTime)
            {
                if (token.IsCancellationRequested)
                    return;
                //_label.fontSize = startFontSize + fontSizeWay * time;
                float scale = startScale + scaleWay * time;
                _targetTransform.localScale = new Vector3(scale, scale, scale);
                await UniTask.NextFrame(cancellationToken: token);
            }

            startScale = _targetTransform.localScale.x;
            scaleWay = (EndScale - startScale) / _endDuration;

            float startAlpha = _label.color.a;
            float alphaWay = (EndAlpha - startAlpha) / _endDuration;

            Color color = new Color(_label.color.r, _label.color.g, _label.color.b, startAlpha);

            for (float time = 0f; time < _endDuration; time += Time.deltaTime)
            {
                if (token.IsCancellationRequested)
                    return;

                if (time > _endDuration / 2f)
                {
                    IsComplete = true;
                }

                color.a = startAlpha + alphaWay * time;
                _label.color = color;
                //_label.fontSize = startFontSize + fontSizeWay * time;
                float scale = startScale + scaleWay * time;
                _targetTransform.localScale = new Vector3(scale, scale, scale);
                await UniTask.NextFrame(cancellationToken: token);
            }

            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(StartLabel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLabel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLabel))]
        private ComponentAttachInfo DefineLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}