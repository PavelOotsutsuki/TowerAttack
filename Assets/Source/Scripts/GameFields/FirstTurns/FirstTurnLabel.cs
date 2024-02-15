using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Tools;

namespace GameFields
{
    public class FirstTurnLabel : MonoBehaviour
    {
        private const float LifeAlpha = 1f;
        private const float EndAlpha = 0f;

        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _startFontSize = 36f;
        [SerializeField] private float _middleFontSize = 120f;
        [SerializeField] private float _endFontSize = 150f;
        [SerializeField] private float _middleDuration = 1f;
        [SerializeField] private float _endDuration = 1f;

        public void Init()
        {
            gameObject.SetActive(false);
            _label.color = new Color(_label.color.r, _label.color.g, _label.color.b, LifeAlpha);
            _label.fontSize = _startFontSize;
        }

        public IEnumerator Activate()
        {
            gameObject.SetActive(true);

            yield return MiddleAnimation();
            yield return EndAnimation();
        }

        private IEnumerator MiddleAnimation()
        {
            float startFontSize = _label.fontSize;
            float fontSizeWay = (_middleFontSize - startFontSize) / _middleDuration;

            for (float time = 0f; time < _middleDuration; time += Time.deltaTime)
            {
                _label.fontSize = startFontSize + fontSizeWay * time;
                yield return null;
            }
        }

        private IEnumerator EndAnimation()
        {
            float startFontSize = _label.fontSize;
            float fontSizeWay = (_endFontSize - startFontSize) / _endDuration;

            float startAlpha = _label.color.a;
            float alphaWay = (EndAlpha - startAlpha) / _endDuration;

            for (float time = 0f; time < _endDuration; time += Time.deltaTime)
            {
                _label.color = new Color(_label.color.r, _label.color.g, _label.color.b, startAlpha + alphaWay * time);
                _label.fontSize = startFontSize + fontSizeWay * time;
                yield return null;
            }
        }


        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineLabel();
        }

        [ContextMenu(nameof(DefineLabel))]
        private void DefineLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }
    }
}
