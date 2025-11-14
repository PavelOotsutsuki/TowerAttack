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

        private ReadOnlyRectTransform _RORCardTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private BigCardRootActivateData _bigCardRootActivateData;
        private CardFrame _cardFrame;

        public bool IsBlock { get; private set; }
        public bool? IsShown { get; private set; } = null;

        internal void Init(CardViewData cardViewData, ReadOnlyRectTransform RORCartTransform,
            CardViewService cardViewService, Vector2 cardSize, CardFrame cardFrame,
            CardCapabilityDescription cardCapabilityDescription)
        {
            _RORCardTransform = RORCartTransform;
            _cardViewService = cardViewService;
            _cardSize = cardSize;
            _cardView.Init(cardCapabilityDescription);
            _cardFrame = cardFrame;

            IsBlock = false;

            DefineSmallSize();
            SetView(cardViewData);
        }

        public void SetView(CardViewData cardViewData)
        {
            _cardView.FillData(cardViewData);

            BigCardShowData bigCardShowData = new BigCardShowData(_cardSize, _RORCardTransform, cardViewData);
            CardDescriptionActivateData cardDescriptionActivateData = new CardDescriptionActivateData(cardViewData.Description);
            CapabilityDescriptionActivateData capabilityDescriptionActivateData = new CapabilityDescriptionActivateData(cardViewData.CardCapability);

            _bigCardRootActivateData = new BigCardRootActivateData(bigCardShowData, cardDescriptionActivateData,
                capabilityDescriptionActivateData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsBlock)
            {
                //Debug.Log("После canvas group.blockRaycasts это не должно вызываться");
                return;
            }

            StartReview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EndReview();
        }

        private void OnDisable()
        {
            if (_cardViewService.IsView(this))
                EndReview();
        }

        internal void StartReview()
        {
            _cardViewService.SetOverview(this, _bigCardRootActivateData, _cardFrame);
        }

        internal void EndReview()
        {
            _cardViewService.SetDefaultView();
        }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs)
        {
            _cardView.RechangeFeature(givenPairs);
        }

        internal void Block()
        {
            if (gameObject.activeSelf)
                _cardFrame.Block();

            IsBlock = true;
        }

        internal void Unblock()
        {
            if (gameObject.activeSelf)
                _cardFrame.Unblock();

            IsBlock = false;
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

        private void DefineSmallSize()
        {
            _RORCardTransform.SetSize(_cardSize);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardFront))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup(),
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