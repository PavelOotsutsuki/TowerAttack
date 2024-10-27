using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class CardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IViewable
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private float _width = 150f;
        [SerializeField] private float _height = 210f;

        [SerializeField] private CardView _cardView;
        [SerializeField] private CardBlock _cardBlock;

        private ReadOnlyRectTransform _readOnlyCardRectTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private BigCardShowData _bigCardShowData;

        public bool IsBlock { get; private set; }

        internal void Init(CardViewConfig cardViewConfig, ReadOnlyRectTransform readOnlyCartRectTransform, CardViewService cardViewService)
        {
            _readOnlyCardRectTransform = readOnlyCartRectTransform;
            _cardViewService = cardViewService;
            _cardSize = new Vector2(_width, _height);

            IsBlock = false;

            _cardView.FillData(cardViewConfig);
            DefineSmallSize();

            _bigCardShowData = new BigCardShowData(_cardSize, _readOnlyCardRectTransform, cardViewConfig);
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
                Debug.Log("После canvas group.blockRaycasts это не должно вызываться");
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
            _cardBlock.Block();

            IsBlock = true;
        }

        internal void Unblock()
        {
            _cardBlock.Unblock();

            IsBlock = false;
        }

        private void DefineSmallSize()
        {
            _readOnlyCardRectTransform.SetSize(_cardSize);
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