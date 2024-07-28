using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class CardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IShowable
    {
        [SerializeField] private float _width = 150f;
        [SerializeField] private float _height = 210f;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;

        private RectTransform _cardRectTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private CardConfig _cardConfig;

        public bool IsBlock { get; private set; }

        internal void Init(CardConfig cardConfig, RectTransform cartRectTransform, CardViewService cardViewService)
        {
            _cardConfig = cardConfig;
            _cardRectTransform = cartRectTransform;
            _cardViewService = cardViewService;
            _cardSize = new Vector2(_width, _height);

            IsBlock = false;

            DefineViewCharacters();
            DefineSmallSize();
        }

        internal void StartReview()
        {
            _cardViewService.SetOverview(this, _cardSize, _cardRectTransform.position.x, _cardConfig);
        }

        internal void EndReview()
        {
            _cardViewService.SetDefaultView();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsBlock)
            {
                return;
            }

            StartReview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EndReview();
        }

        internal void Block()
        {
            _frameImage.color = _disableFrameColor;
            IsBlock = true;
        }

        internal void Unblock()
        {
            _frameImage.color = _enableFrameColor;
            IsBlock = false;
        }

        private void DefineSmallSize()
        {
            _cardRectTransform.sizeDelta = _cardSize;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        private void DefineViewCharacters()
        {
            _icon.sprite = _cardConfig.Icon;
            _number.text = _cardConfig.Number.ToString();
            _name.text = _cardConfig.Name;
            _feature.text = _cardConfig.Feature;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}