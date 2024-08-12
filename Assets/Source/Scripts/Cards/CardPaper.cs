using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardPaper : MonoBehaviour, ICardState
    {
        private const SideType DefaultSide = SideType.Back;
        private const bool DefaultInteractionActive = false;

        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;

        internal void Init(Card me, CardViewService cardViewService, CardConfig cardConfig, Transform dragContainer, RectTransform cardTransform)
        {
            _cardFront.Init(cardConfig, cardTransform, cardViewService);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, me);
            _cardDragAndDrop.Init(cardTransform, _cardDragAndDropActions, dragContainer);

            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop);

            SetSide(DefaultSide);
            SetActiveInteraction(DefaultInteractionActive);
        }

        public void EndDrag()
        {
            _cardDragAndDrop.BlockDrag();
        }

        public void SetDragAndDropListener(ICardDragAndDropListener cardDragAndDropListener)
        {
            _cardDragAndDropActions.SetListener(cardDragAndDropListener);
        }

        public void SetSide(SideType sideType)
        {
            switch (sideType)
            {
                case SideType.Front:
                    _cardSideFlipper.SetFrontSide();
                    break;
                case SideType.Back:
                    _cardSideFlipper.SetBackSide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown side type: {sideType}");
            }
        }

        public void SetActiveInteraction(bool isActive)
        {
            if (isActive == false)
            {
                _cardSideFlipper.DeactivateInteraction();
                return;
            }

            if (_cardDragAndDrop.IsDragable == false)
            {
                _cardSideFlipper.ActivateInteraction();
            }
        }

        public void View()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardBack();
            DefineCardFront();
            DefineCardDragAndDrop();
        }

        [ContextMenu(nameof(DefineCardDragAndDrop))]
        private void DefineCardDragAndDrop()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardBack))]
        private void DefineCardBack()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardBack, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFront))]
        private void DefineCardFront()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardFront, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
