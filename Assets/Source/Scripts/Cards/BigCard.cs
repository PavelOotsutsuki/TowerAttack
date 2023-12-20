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

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasScaler _canvasScaler;

        private float _bigHeight;
        private float _bigWidth;
        private float _sizeFactor;
        private float _canvasHeight;
        private float _screenFactor;

        internal void Init()
        {
            _rectTransform.rotation = Quaternion.identity;
            _canvasHeight = _canvasScaler.referenceResolution.y;

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
            _sizeFactor = cardSize.Width / cardSize.Height;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(positionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
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