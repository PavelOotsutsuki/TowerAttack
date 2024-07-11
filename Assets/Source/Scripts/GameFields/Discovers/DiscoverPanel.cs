using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    public class DiscoverPanel : MonoBehaviour
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

            Color startColor = new(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);

            _panel.color = startColor;
        }

        public void Activate()
        {
            _panel.raycastTarget = true;

            Color activateColor = new(_panel.color.r, _panel.color.g, _panel.color.b, ActiveAlpha / MaxAlpha);

            _panel.DOColor(activateColor, _activateDuration);
        }

        public void Deactivate()
        {
            //float startAlpha = _panel.color.a;
            //float alphaWay = (DeactiveAlpha - startAlpha) / _deactivateDuration;

            //for (float time = 0f; time < _deactivateDuration; time += Time.deltaTime)
            //{
            //    _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, startAlpha + alphaWay * time);
            //    yield return null;
            //}
            Color deactivateColor = new(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha / MaxAlpha);

            _panel.DOColor(deactivateColor, _deactivateDuration)
            .OnComplete(() =>
            {
                _panel.raycastTarget = false;
            });
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
