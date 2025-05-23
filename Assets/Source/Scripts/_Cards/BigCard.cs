using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.FillComponents;
using Tools;
using System.Collections.Generic;
using Tools.Utils.Screens;

namespace Cards
{
    public class BigCard : MonoBehaviour, IViewable<BigCardShowData>, IAutomaticFillComponents
    {
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        //[SerializeField] private CanvasScaler _canvasScaler;

        private float _bigHeight;
        private float _bigWidth;
        private float _sizeFactor;
        private float _canvasHeight;
        private float _screenFactor;

        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _rectTransform.rotation = Quaternion.identity;
            _canvasHeight = ScreenView.Y();

            Hide();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            gameObject.SetActive(false);
        }

        public void Show(BigCardShowData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _cardView.FillData(data.CardViewData);
            _sizeFactor = data.CardSize.x / data.CardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(data.PositionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
            gameObject.SetActive(true);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(BigCard))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}