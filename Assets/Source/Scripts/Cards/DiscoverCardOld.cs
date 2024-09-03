using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    internal class DiscoverCardOld : MonoBehaviour//, ICardState
    {
        //[SerializeField, Min(1f)] private float _scaleFactor = 2f;

        //[SerializeField] private Image _icon;
        //[SerializeField] private TMP_Text _number;
        //[SerializeField] private TMP_Text _name;
        //[SerializeField] private TMP_Text _feature;
        //[SerializeField] private RectTransform _rectTransform;
        //[SerializeField] private float _growDuration = 1f;
        ////[SerializeField] private CanvasScaler _canvasScaler;

        //private RectTransform _cardTransform;
        //private ICardState _cardPaper;
        //private Movement _movement;
        //private float _bigHeight;
        //private float _bigWidth;
        //private float _canvasHeight;
        ////private float _screenFactor;


        //internal void Init(CardViewConfig cardViewConfig, RectTransform cardTransform, ICardState cardPaper)
        //{
        //    _rectTransform.rotation = Quaternion.identity;
        //    _rectTransform.localPosition = Vector3.zero;
        //    _canvasHeight = ScreenView.GetFactorX() * Screen.height;
        //    _cardTransform = cardTransform;
        //    _cardPaper = cardPaper;
        //    _movement = new Movement(_rectTransform);

        //    //_sizeFactor = _cardTransform.sizeDelta.x / _cardTransform.sizeDelta.y;
        //    //_bigHeight = _canvasHeight / _scaleFactor;
        //    //_bigWidth = _bigHeight * _sizeFactor;

        //    //_sizeFactor = _cardTransform.sizeDelta.x / _cardTransform.sizeDelta.y;
        //    _bigHeight = _cardTransform.sizeDelta.y * _scaleFactor;
        //    _bigWidth = _cardTransform.sizeDelta.x * _scaleFactor;
        //    //_screenFactor = Screen.height / _canvasHeight;

        //    _icon.sprite = cardViewConfig.Icon;
        //    _number.text = cardViewConfig.Number.ToString();
        //    _name.text = cardViewConfig.Name;
        //    _feature.text = cardViewConfig.Feature;

        //    Hide();
        //}

        //public void Hide()
        //{
        //    gameObject.SetActive(false);
        //    _cardPaper.Hide();
        //}

        //public void View()
        //{
        //    //_rectTransform.position = new Vector2(_rectTransform.position.x, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
        //    _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
        //    _rectTransform.localPosition = Vector3.zero;
        //    //_rectTransform.sizeDelta = new Vector2(0f, 0f);
        //    Vector3 defaultScale = _rectTransform.localScale;
        //    _movement.MoveInstantly(_rectTransform.position, Quaternion.identity.eulerAngles, Vector3.zero);
        //    _movement.MoveSmoothly(_rectTransform.position, Quaternion.identity.eulerAngles, _growDuration, defaultScale);
        //    gameObject.SetActive(true);
        //    _cardPaper.View();
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents))]
        //private void DefineAllComponents()
        //{
        //    DefineRectTransform();
        //}

        //[ContextMenu(nameof(DefineRectTransform))]
        //private void DefineRectTransform()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        //}
        //#endregion 
    }
}
