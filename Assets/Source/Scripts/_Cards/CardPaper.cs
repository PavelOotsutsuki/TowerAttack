using UnityEngine;
using Tools.Utils.FillComponents;
using Tools;
using System.Collections.Generic;
using Tools.Settings;

namespace Cards
{
    public class CardPaper : MonoBehaviour, ICardState, IAutomaticFillComponents
    {
        private const SideType DefaultSide = SideType.Back;
        private const bool DefaultInteractionActive = false;

        //[SerializeField] private float _backSizeFactor = 1.0641f;
        //[SerializeField] private float _backSizeFactor = 1f;
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardFrame _cardFrame;
        //[SerializeField] private CardFire _cardFire;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;

        public bool? IsShown { get; private set; } = null;
        public SideType CurrentSide => _cardSideFlipper.CurrentSide;

        internal void Init(Card me, CardViewService cardViewService, CardViewConfig cardViewConfig,
            RectTransform cardTransform, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            ReadOnlyRectTransform readOnlyRectTransform = new ReadOnlyRectTransform(cardTransform);

            Vector2 cardSizeFront = Settings.CardSize;
            Vector2 cardSizeBack = Settings.CardSize;

            _cardFront.Init(cardViewConfig, readOnlyRectTransform, cardViewService, cardSizeFront);
            _cardBack.Init(cardSizeBack);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, me, cardDragAndDropHandler);
            _cardDragAndDrop.Init(cardTransform, _cardDragAndDropActions);

            _cardFrame.Init();
            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop, _cardFrame);

            SetSide(DefaultSide);
            SetActiveInteraction(DefaultInteractionActive);
        }

        public void EndDrag()
        {
            _cardDragAndDrop.BlockDrag();
        }

        //public void SetDragAndDropHandler(ICardDragAndDropHandler cardDragAndDropHandler)
        //{
        //    _cardDragAndDropActions.SetListener(cardDragAndDropHandler);
        //}

        public void Fire()
        {
            //switch (_cardSideFlipper.CurrentSide)
            //{
            //    case SideType.Front:
            //        _cardFront.Fire();
            //        break;
            //    case SideType.Back:
            //        _cardFront.Fire();
            //        break;
            //    default:
            //        throw new System.Exception("Неизвестный тип side карты");
            //}

            _cardFrame.Fire();
        }

        public void SetSide(SideType sideType)
        {
            _cardSideFlipper.SetSide(sideType);
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
            else
            {
                Debug.Log("Попадаю сюда когда отпустил драгнутую карту, но успел драгнуть и отпустить вторую до того как первая вернулась в руку");
            }
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardPaper))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardBack(),
                DefineCardFront(),
                DefineCardDragAndDrop(),
                DefineCardFrame()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardDragAndDrop))]
        private ComponentAttachInfo DefineCardDragAndDrop()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardBack))]
        private ComponentAttachInfo DefineCardBack()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardBack, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFront))]
        private ComponentAttachInfo DefineCardFront()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFront, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFrame))]
        private ComponentAttachInfo DefineCardFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFrame, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}
