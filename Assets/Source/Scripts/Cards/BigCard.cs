using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tools;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class BigCard : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;
        [SerializeField] private float _reviewOffsetFactor = 2f;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        internal void Init()
        {
            _rectTransform.rotation = Quaternion.identity;

            Hide();
        }

        internal void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        internal void Show(CardSize cardSize, float positionX, CardSO cardSO)
        {
            _icon.sprite = cardSO.Icon;
            _number.text = cardSO.Number.ToString();
            _name.text = cardSO.Name;
            _feature.text = cardSO.Feature;
            _rectTransform.position = new Vector2(positionX, cardSize.Height / _reviewOffsetFactor + cardSize.Height);
            _rectTransform.sizeDelta = new Vector2(cardSize.Width * _scaleFactor, cardSize.Height * _scaleFactor);
            _canvasGroup.alpha = 1;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCanvasGroup();
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}