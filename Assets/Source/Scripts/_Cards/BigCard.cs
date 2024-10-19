using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.FillComponents;
using Tools;

namespace Cards
{
    internal class BigCard : MonoBehaviour, IViewable<BigCardShowData>
    {
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        [SerializeField] private CardView _cardView;
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

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(BigCardShowData data)
        {
            _cardView.FillData(data.CardViewConfig);
            _sizeFactor = data.CardSize.x / data.CardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(data.PositionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
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