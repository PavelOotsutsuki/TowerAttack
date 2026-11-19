using UnityEngine;
using Tools.Utils.FillComponents;
using Tools;
using System.Collections.Generic;
using Tools.Utils.Screens;
using Cards.Views.BigCardViews.Capabilities;
using System;

namespace Cards.Views.BigCardViews.BigCards
{
    public class BigCard : MonoBehaviour, IViewable<BigCardShowData>, IAutomaticFillComponents
    {
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;

        private float _bigHeight;
        private float _bigWidth;
        private float _sizeFactor;
        private float _canvasHeight;
        private float _screenFactorHeight;
        private float _screenFactorWidth;

        public bool? IsShown { get; private set; } = false;

        internal void Init(CardCapabilityDescription cardCapabilityDescription)
        {
            _rectTransform.rotation = Quaternion.identity;
            _canvasHeight = ScreenView.Y();
            _cardView.Init(cardCapabilityDescription);

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

            switch (data.PositionType)
            {
                case BigCardPositionType.RightUp:
                    ShowOnRightUp();
                    break;
                case BigCardPositionType.InsteadOfTarget:
                    ShowOnInsteadOfTarget(data.CardSize.x, data.CardSize.y, data.PositionX);
                    break;
                case BigCardPositionType.LeftOfTarget:
                    ShowOnLeftOfTarget(data);
                    break;
                default:
                    throw new NotImplementedException($"Неизвестный {nameof(BigCardPositionType)}: {data.PositionType}");
            }

            #region DEBUG
            //Debug.Log("data.CardSize.x: " + data.CardSize.x); // 150
            //Debug.Log("data.CardSize.y: " + data.CardSize.y); // 210
            //Debug.Log("_canvasHeight: " + _canvasHeight);     // 1080
            //Debug.Log("_scaleFactor: " + _scaleFactor);       // 1.5
            //Debug.Log("_bigHeight: " + _bigHeight);           // 720
            //Debug.Log("_bigWidth: " + _bigWidth);             // 514.2857
            //Debug.Log("Screen.height: " + Screen.height);     // 2160
            //Debug.Log("_screenFactor: " + _screenFactor);     // 2
            //Debug.Log("_rectTransform.position: " + _rectTransform.position); // (720.00, 1560.00, 0.00)
            #endregion

            gameObject.SetActive(true);
        }

        private void ShowOnRightUp()
        {
            _rectTransform.anchorMin = new Vector2(1f, 1f);
            _rectTransform.anchorMax = new Vector2(1f, 1f);

            _rectTransform.pivot = new Vector2(1f, 1f);
            _rectTransform.anchoredPosition = Vector2.zero;
        }

        private void ShowOnInsteadOfTarget(float cardSizeX, float cardSizeY, float positionX)
        {
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            _sizeFactor = cardSizeX / cardSizeY;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactorHeight = Screen.height / _canvasHeight;
            //_screenFactorWidth = Screen.width / ScreenView.X();
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);

            _rectTransform.position = new Vector2(positionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactorHeight);
        }

        private void ShowOnLeftOfTarget(BigCardShowData data)
        {
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            float positionX = data.PositionX;
            float positionY = data.PositionY;
            float width = data.Width;

            _sizeFactor = data.CardSize.x / data.CardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactorWidth = Screen.width / ScreenView.X();


            _rectTransform.position = new Vector2(positionX - width - (_bigWidth /2f), positionY);



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