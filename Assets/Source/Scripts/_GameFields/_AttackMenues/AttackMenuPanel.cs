using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.AttackMenues
{
    public class AttackMenuPanel : MonoBehaviour
    {
        private const float MaxAlpha = 255f;
        private const float DeactiveAlpha = 0f;

        [SerializeField] private Image _panel;
        [SerializeField] private float _activateDuration = 1f;
        [SerializeField] private float _deactivateDuration = 2f;
        [SerializeField, Range(DeactiveAlpha, MaxAlpha)] private float _activeAlpha = 248f;

        public void Init()
        {
            _panel.raycastTarget = false;

            Color startColor = new(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);

            _panel.color = startColor;
        }

        public void Activate()
        {
            _panel.raycastTarget = true;

            Color activateColor = new(_panel.color.r, _panel.color.g, _panel.color.b, _activeAlpha / MaxAlpha);

            _panel.DOColor(activateColor, _activateDuration);
        }

        public void Deactivate(Action activateCallback)
        {
            Color deactivateColor = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha / MaxAlpha);

            _panel.DOColor(deactivateColor, _deactivateDuration)
            .OnComplete(() =>
            {
                _panel.raycastTarget = false;
                activateCallback.Invoke();
            });
        }

        #region AutomaticFillComponents

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

        #endregion
    }
}
