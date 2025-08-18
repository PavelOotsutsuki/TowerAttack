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
        //[SerializeField] private CardBlock _cardBlock;

        private ReadOnlyRectTransform _readOnlyCardRectTransform;
        private CardViewService _cardViewService;
        private Vector2 _cardSize;
        private BigCardShowData _bigCardShowData;
        //private ICardBlockable _cardBlockable;
        private CardFrame _cardFrame;

        public bool IsBlock { get; private set; }
        public bool? IsShown { get; private set; } = null;

        internal void Init(CardViewData cardViewData, ReadOnlyRectTransform readOnlyCartRectTransform,
            CardViewService cardViewService, Vector2 cardSize, CardFrame cardFrame)
        {
            _readOnlyCardRectTransform = readOnlyCartRectTransform;
            _cardViewService = cardViewService;
            _cardSize = cardSize;
            //_cardBlockable = cardBlockable;
            _cardFrame = cardFrame;
            //_cardFrame.Init();

            IsBlock = false;

            _cardView.FillData(cardViewData);
            DefineSmallSize();

            _bigCardShowData = new BigCardShowData(_cardSize, _readOnlyCardRectTransform, cardViewData);
        }

        private void OnDisable()
        {
            if (_cardViewService.IsView(this))
                EndReview();
        }

        internal void StartReview()
        {
            _cardViewService.SetOverview(this, _bigCardShowData, _cardFrame);
        }

        internal void EndReview()
        {
            _cardViewService.SetDefaultView();
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

        //public void Fire()
        //{
        //    _cardFrame.Fire();
        //}
        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs)
        {
            _cardView.RechangeFeature(givenPairs);
        }

        internal void Block()
        {
            //_cardBlock.Block();
            //_cardBlockable.Block();
            if (gameObject.activeSelf)
                _cardFrame.Block();

            IsBlock = true;
        }

        internal void Unblock()
        {
            //_cardBlock.Unblock();
            //_cardBlockable.Unblock();
            if (gameObject.activeSelf)
                _cardFrame.Unblock();

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
            //_cardBlockable.Show();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _canvasGroup.alpha = 0;
            //_cardBlockable.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardFront))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup(),
                //DefineCardFrame()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        //[ContextMenu(nameof(DefineCardFrame))]
        //private ComponentAttachInfo DefineCardFrame()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _cardFrame, ComponentLocationTypes.InChildren);
        //}
        #endregion 
    }
}