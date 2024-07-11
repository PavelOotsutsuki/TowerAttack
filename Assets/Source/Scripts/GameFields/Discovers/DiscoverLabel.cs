using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DiscoverLabel : MonoBehaviour
    {
        private const float LifeAlpha = 1f;
        private const float EndAlpha = 0f;

        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _startFontSize = 0f;
        [SerializeField] private float _middleFontSize = 60f;
        [SerializeField] private float _endFontSize = 70f;
        [SerializeField] private float _middleDuration = 0.3f;
        [SerializeField] private float _endDuration = 0.3f;

        public void Init()
        {
            gameObject.SetActive(false);

            Color startColor = new(_label.color.r, _label.color.g, _label.color.b, LifeAlpha);

            _label.color = startColor;
            _label.fontSize = _startFontSize;
        }

        public void Activate(string message)
        {
            Activating(message).ToUniTask();
        }

        private IEnumerator Activating(string message)
        {
            gameObject.SetActive(true);
            _label.text = message;

            float startFontSize = _label.fontSize;
            float fontSizeWay = (_middleFontSize - startFontSize) / _middleDuration;

            for (float time = 0f; time < _middleDuration; time += Time.deltaTime)
            {
                _label.fontSize = startFontSize + fontSizeWay * time;
                yield return null;
            }

            startFontSize = _label.fontSize;
            fontSizeWay = (_endFontSize - startFontSize) / _endDuration;

            float startAlpha = _label.color.a;
            float alphaWay = (EndAlpha - startAlpha) / _endDuration;

            Color color = new(_label.color.r, _label.color.g, _label.color.b, startAlpha);

            for (float time = 0f; time < _endDuration; time += Time.deltaTime)
            {
                color.a = startAlpha + alphaWay * time;
                _label.color = color;
                _label.fontSize = startFontSize + fontSizeWay * time;
                yield return null;
            }

            gameObject.SetActive(false);
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
