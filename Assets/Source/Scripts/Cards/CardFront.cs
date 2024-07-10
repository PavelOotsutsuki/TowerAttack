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
        private CardSize _cardSize;
        private CardSO _cardSO;
        private bool _isBlock;

        private PointerEventData _currentEventData;

        public bool IsBlock => _isBlock;

        internal void Init(CardSO cardSO, RectTransform cartRectTransform, CardViewService cardViewService)
        {
            _isBlock = false;
            _cardSize = new CardSize(_width, _height);
            _cardRectTransform = cartRectTransform;
            _cardViewService = cardViewService;
            _cardSO = cardSO;

            DefineViewCharacters();
            DefineSmallSize();
        }

        internal void StartReview()
        {
            _cardViewService.SetOverview(this, _cardSize, _cardRectTransform.position.x, _cardSO);
        }

        internal void EndReview()
        {
            _cardViewService.SetDefaultView();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentEventData = eventData;

            if (_isBlock)
            {
                return;
            }

            StartReview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentEventData = eventData;

            EndReview();
        }

        internal void Block()
        {
            _frameImage.color = _disableFrameColor;
            _isBlock = true;
        }

        internal void Unblock()
        {
            _frameImage.color = _enableFrameColor;
            _isBlock = false;
        }

        private void DefineSmallSize()
        {
            _cardRectTransform.sizeDelta = new Vector2(_cardSize.Width, _cardSize.Height);
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
            _icon.sprite = _cardSO.Icon;
            _number.text = _cardSO.Number.ToString();
            _name.text = _cardSO.Name;
            _feature.text = _cardSO.Feature;
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