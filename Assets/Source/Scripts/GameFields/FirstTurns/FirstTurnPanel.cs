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
        private const float DeactiveAlpha = 0f;
        private const float ActiveAlpha = 211f;

        [SerializeField] private Image _panel;
        [SerializeField] private float _activateDuration = 1f;

        public void Init()
        {
            _panel.raycastTarget = true;
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);
        }

        public IEnumerator Activate()
        {
            _panel.raycastTarget = false;

            //_panel.DOColor(new Color(_panel.color.r, _panel.color.g, _panel.color.b, ActiveAlpha), _activateDuration);

            for (float time = 0f; time < _activateDuration; time += Time.deltaTime)
            {
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, time / _activateDuration / 255f * ActiveAlpha);
                yield return null;
            }
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
