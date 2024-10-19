using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class CardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IViewable
    {
        [SerializeField] private float _width = 150f;
        [SerializeField] private float _height = 210f;

        [SerializeField] private CardView _cardView;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;

        private RectTransform _cardRectTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private BigCardShowData _bigCardShowData;

        public bool IsBlock { get; private set; }

        internal void Init(CardViewConfig cardViewConfig, RectTransform cartRectTransform, CardViewService cardViewService)
        {
            _cardRectTransform = cartRectTransform;
            _cardViewService = cardViewService;
            _cardSize = new Vector2(_width, _height);

            IsBlock = false;

            _cardView.FillData(cardViewConfig);
            DefineSmallSize();

            ReadOnlyTransform readOnlyTransform = new ReadOnlyTransform(_cardRectTransform);
            _bigCardShowData = new BigCardShowData(_cardSize, readOnlyTransform, cardViewConfig);
        }

        internal void StartReview()
        {
            _cardViewService.SetOverview(this, _bigCardShowData);
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

        #region AutomaticFillComponents
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
        #endregion 
    }
}