using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields
{
    public class FirstTurnPanel : MonoBehaviour
    {
        private const float DeactiveAlpha = 0f;
        private const float ActiveAlpha = 211f;

        [SerializeField] private Image _panel;

        public void Init()
        {
            _panel.raycastTarget = true;
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);
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
