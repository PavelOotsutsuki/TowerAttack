using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameFields
{
    public class FirstTurnPanel : MonoBehaviour
    {
        private const float MaxAlpha = 255f;
        private const float DeactiveAlpha = 0f;
        private const float ActiveAlpha = 211f;

        [SerializeField] private Image _panel;
        [SerializeField] private float _activateDuration = 1f;
        [SerializeField] private float _deactivateDuration = 1f;

        public void Init()
        {
            _panel.raycastTarget = false;
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);
        }

        public IEnumerator Activate()
        {
            _panel.raycastTarget = true;

            //_panel.DOColor(new Color(_panel.color.r, _panel.color.g, _panel.color.b, ActiveAlpha), _activateDuration);

            float startAlpha = _panel.color.a;
            float alphaWay = (ActiveAlpha - startAlpha) / _activateDuration / MaxAlpha;

            for (float time = 0f; time < _activateDuration; time += Time.deltaTime)
            {
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, startAlpha + alphaWay * time);
                yield return null;
            }
        }

        public IEnumerator Deactivate()
        {
            float startAlpha = _panel.color.a;
            float alphaWay = (DeactiveAlpha - startAlpha) / _deactivateDuration;

            for (float time = 0f; time < _deactivateDuration; time += Time.deltaTime)
            {
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, startAlpha + alphaWay * time);
                yield return null;
            }

            _panel.raycastTarget = false;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefinePanel();
        }

        [ContextMenu(nameof(DefinePanel))]
        private void DefinePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InThis);
        }
    }
}
