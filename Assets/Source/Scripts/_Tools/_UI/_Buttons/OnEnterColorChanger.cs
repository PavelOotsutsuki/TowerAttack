using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools.UI.Buttons
{
    internal class OnEnterColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _pressedColor;

        private Image _target;

        public void Init(Image target)
        {
            _target = target;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _target.color = _selectColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _target.color = _pressedColor;
        }
    }
}
