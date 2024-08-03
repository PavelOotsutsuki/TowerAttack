using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class Card : MonoBehaviour, ICardTransformable
    {
        private const SideType DefaultSide = SideType.Back;
        private const bool DefaultInteractionActive = false;

        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardConfig _cardConfig;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;
        private CardCharacter _createdCharacter;
        private IEffectFactory _effectFactory; 

        public RectTransform Transform => _rectTransform;
        public Vector3 DefaultScaleVector => _defaultScaleVector;
        public CardCharacter Character { get; private set; }
        public int EffectCounter { get; private set; } 

        internal void Init(IEffectFactory effectFactory, CardViewService cardViewService, Transform dragContainer)
        {
            _effectFactory = effectFactory;
            
            _rectTransform.localScale = _defaultScaleVector;
            _cardFront.Init(_cardConfig, _rectTransform, cardViewService);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, this);
            _cardDragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);

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

        public void Play()
        {
            if (_createdCharacter == null)
            {
                CreateCardCharacter();
            }

            Deactivate();
            _effectFactory.Create(_cardConfig.Effect.Type);
            EffectCounter = _cardConfig.Effect.Duration;
            Character = _createdCharacter;
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

        public void DiscardCard()
        {
            Activate();
            
            _createdCharacter.Disable();
            Character = null;
        }

        public Vector3 GetCardCharacterPosition()
        {
            if (_createdCharacter == null)
            {
                throw new NullReferenceException("СardCharacter is not instantiate");
            }

            return _createdCharacter.GetPosition();
        }

        public void DecreaseCounter() => EffectCounter--;

        private void CreateCardCharacter()
        {
            _createdCharacter = Instantiate(_cardConfig.CardCharacter);
            _createdCharacter.Init(_cardConfig.AwakeSound);
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
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
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineCardDragAndDrop))]
        private void DefineCardDragAndDrop()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
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
