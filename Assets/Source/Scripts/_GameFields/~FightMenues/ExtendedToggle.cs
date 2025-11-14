using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFields.FightMenues
{
    public class ExtendedToggle : Toggle
    {
        private Action<RectTransform> _onSelect;

        public void Init(Action<RectTransform> onSelect)
        {
            _onSelect = onSelect;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            _onSelect?.Invoke(gameObject.transform as RectTransform);

            base.OnSelect(eventData);
        }
    }
}