using UnityEngine;
using Tools.Utils.FillComponents;
using Tools;
using System.Collections.Generic;
using Tools.Settings;
using Cards.Animations;
using Cards.Animations.Curses;
using Cards.Animations.Fires;
using Cards.Views;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Sounds;
using System.Threading;

namespace Cards.Insides
{
    [RequireComponent(typeof(CardFireAnimator))]
    internal class CardPaper : MonoBehaviour, IFeatureWatcher, ICardState, IAutomaticFillComponents
    {
        private const SideType DefaultSide = SideType.Back;
        private const bool DefaultInteractionActive = false;

        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardFrame _cardFrame;
        [SerializeField] private CardSpriteManager _cardSpriteManager;
        [SerializeField] private CardFireAnimator _cardFireAnimator;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;
        private OnFireLogic _onFireLogic;

        public bool? IsShown { get; private set; } = null;
        public SideType CurrentSide => _cardSideFlipper.CurrentSide;
        public bool IsFired => _onFireLogic.IsActive == true;
        public string Feature => _cardFront.Feature;

        internal void Init(Card me, CardViewService cardViewService, CardViewData cardViewData,
            RectTransform cardTransform, ICardDragAndDropHandler cardDragAndDropHandler,
            CardSpriteModeManager cardSpriteModeManager, CurseAnimator curseAnimator,
            CardCapabilityDescription cardCapabilityDescription, CardSoundRoot cardSoundRoot,
            CardSoundLogic cardSoundLogic, CancellationToken cardToken)
        {
            ReadOnlyRectTransform readOnlyRectTransform = new ReadOnlyRectTransform(cardTransform);

            Vector2 cardSizeFront = GameSettings.CardSize;
            Vector2 cardSizeBack = GameSettings.CardSize;

            _cardSpriteManager.Init(cardSpriteModeManager, curseAnimator);
            _cardFront.Init(cardViewData, readOnlyRectTransform, cardViewService, cardSizeFront, _cardFrame,
                cardCapabilityDescription, cardToken);
            _cardBack.Init(cardSizeBack);
            _cardFireAnimator.Init();
            _onFireLogic = new OnFireLogic(_cardFireAnimator, cardSoundRoot, cardSoundLogic);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, me, cardDragAndDropHandler);
            _cardDragAndDrop.Init(cardTransform, _cardDragAndDropActions, cardToken);

            _cardFrame.Init();
            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop, _cardFrame, _cardSpriteManager);

            SetSide(DefaultSide);
            SetActiveInteraction(DefaultInteractionActive);
        }

        public void ResetDrag()
        {
            _cardDragAndDrop.ResetDrag();
        }

        public void EndDrag()
        {
            _cardDragAndDrop.BlockDrag();
        }

        public void Fire(OnFireLogicActivateData onFireLogicActivateData)
        {
            _onFireLogic.Activate(onFireLogicActivateData);
        }

        public void RiseFromTheAshes(CancellationTokenData tokenData)
        {
            _onFireLogic.Deactivate(tokenData);
        }

        public void SetView(CardViewData cardViewData)
        {
            _cardFront.SetView(cardViewData);
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
                DefineCardFireAnimator()
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

        [ContextMenu(nameof(DefineCardFireAnimator))]
        private ComponentAttachInfo DefineCardFireAnimator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFireAnimator, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}