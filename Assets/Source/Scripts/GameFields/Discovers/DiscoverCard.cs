using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCard : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _growDuration = 1f;

        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Movement _movement;
        private Action _clickCallback;
        private float _scaleFactor;

        public void Init(Action clickCallback, float scaleFactor)
        {
            _scaleFactor = scaleFactor;
            _rectTransform.rotation = Quaternion.identity;
            _rectTransform.localPosition = Vector3.zero;
            _clickCallback = clickCallback;
            _movement = new Movement(_rectTransform);

            Hide();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCallback?.Invoke();
        }

        public void Hide()
        {
            Block();
            gameObject.SetActive(false);
        }

        public void Activate(CardViewConfig cardViewConfig, float cardHeight, float cardWidth, float waitDuration = 0f)
        {
            Block();
            _icon.sprite = cardViewConfig.Icon;
            _number.text = cardViewConfig.Number.ToString();
            _name.text = cardViewConfig.Name;
            _feature.text = cardViewConfig.Feature;

            float bigHeight = cardHeight * _scaleFactor;
            float bigWidth = cardWidth * _scaleFactor;

            _rectTransform.sizeDelta = new Vector2(bigWidth, bigHeight);
            _rectTransform.localPosition = Vector3.zero;

            Vector3 defaultScale = _rectTransform.localScale;
            _movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, Vector3.zero);
            _movement.MoveLocalSmoothly(Vector3.zero, Quaternion.identity.eulerAngles, _growDuration, defaultScale);

            gameObject.SetActive(true);

            if (Mathf.Approximately(waitDuration, 0f))
            {
                Unblock();
                return;
            }

            WaitingToUnblock(waitDuration).ToUniTask();
        }

        private IEnumerator WaitingToUnblock(float waitDuration)
        {
            yield return new WaitForSeconds(_growDuration + waitDuration);

            Unblock();
        }

        private void Block()
        {
            _frameImage.color = _disableFrameColor;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Unblock()
        {
            _frameImage.color = _enableFrameColor;
            _canvasGroup.blocksRaycasts = true;
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
