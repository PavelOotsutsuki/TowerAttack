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
        [SerializeField] private OnFireLogic _onFireLogic;
        [SerializeField] private CardSpriteManager _cardSpriteManager;
        //[SerializeField] private CardFireAnimator _cardFireAnimator;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;

        public bool? IsShown { get; private set; } = null;
        public SideType CurrentSide => _cardSideFlipper.CurrentSide;
        public bool IsFired => _onFireLogic.IsActive == true;

        internal void Init(Card me, CardViewService cardViewService, CardViewData cardViewData,
            RectTransform cardTransform, ICardDragAndDropHandler cardDragAndDropHandler, CardCapability cardCapability,
            CardSpriteModeManager cardSpriteModeManager, CurseAnimator curseAnimator)
        {
            ReadOnlyRectTransform readOnlyRectTransform = new ReadOnlyRectTransform(cardTransform);

            Vector2 cardSizeFront = GameSettings.CardSize;
            Vector2 cardSizeBack = GameSettings.CardSize;

            _cardSpriteManager.Init(cardSpriteModeManager, curseAnimator);
            _cardFront.Init(cardViewData, readOnlyRectTransform, cardViewService, cardSizeFront, _cardFrame, cardCapability);
            _cardBack.Init(cardSizeBack);
            _onFireLogic.Init();

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, me, cardDragAndDropHandler);
            _cardDragAndDrop.Init(cardTransform, _cardDragAndDropActions);

            _cardFrame.Init();
            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop, _cardFrame, _cardSpriteManager);

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

        public void Fire(WaitForSeconds delay)
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
            OnFireLogicActivateData onFireLogicActivateData = new OnFireLogicActivateData(delay);
            _onFireLogic.Activate(onFireLogicActivateData);
        }

        public void RiseFromTheAshes()
        {
            _onFireLogic.Deactivate();
        }

        public void SetView(CardViewData cardViewData, CardCapability cardCapability)
        {
            _cardFront.SetView(cardViewData, cardCapability);
        }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs)
        {
            _cardFront.RechangeFeature(givenPairs);
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
                DefineCardFrame(),
                DefineOnFireLogic()
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

        [ContextMenu(nameof(DefineOnFireLogic))]
        private ComponentAttachInfo DefineOnFireLogic()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _onFireLogic, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}
