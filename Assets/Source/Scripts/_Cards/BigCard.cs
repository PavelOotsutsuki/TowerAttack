using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.FillComponents;

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
            gameObject.SetActive(false);
        }

        internal void Show(Vector2 cardSize, float positionX, CardViewConfig cardViewConfig)
        {
            _icon.sprite = cardViewConfig.Icon;
            _number.text = cardViewConfig.Number.ToString();
            _name.text = cardViewConfig.Name;
            _feature.text = cardViewConfig.Feature;
            _sizeFactor = cardSize.x / cardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(positionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
            gameObject.SetActive(true);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}