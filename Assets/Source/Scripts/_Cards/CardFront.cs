using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class CardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IViewable, IAutomaticFillComponents
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private CardView _cardView;
        [SerializeField] private CardBlock _cardBlock;

        private ReadOnlyRectTransform _readOnlyCardRectTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private BigCardShowData _bigCardShowData;

        public bool IsBlock { get; private set; }
        public bool? IsShown { get; private set; } = null;

        internal void Init(CardViewConfig cardViewConfig, ReadOnlyRectTransform readOnlyCartRectTransform,
            CardViewService cardViewService, Vector2 cardSize)
        {
            _readOnlyCardRectTransform = readOnlyCartRectTransform;
            _cardViewService = cardViewService;
            _cardSize = cardSize;

            IsBlock = false;

            _cardView.FillData(cardViewConfig);
            DefineSmallSize();

            _bigCardShowData = new BigCardShowData(_cardSize, _readOnlyCardRectTransform, cardViewConfig);
        }

        private void OnDisable()
        {
            if (_cardViewService.IsView(this))
                EndReview();
            //if (_cardViewService.IsView(this))
            //{
            //    EndReview();
            //    Debug.Log("Эта карта");
            //}
            //else
            //{
            //    Debug.Log("Не эта карта");
            //}
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
            if (IsShown == true)
                return;

            IsShown = true;

            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _canvasGroup.alpha = 0;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}