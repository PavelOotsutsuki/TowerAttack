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
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;
        private CardCharacter _cardCharacter;

        public RectTransform Transform => _rectTransform;
        public Vector3 DefaultScaleVector => _defaultScaleVector;
        public EffectType Effect => _cardSO.Effect;
        public CardCharacter CardCharacter => _cardCharacter;

        internal void Init(CardViewService cardViewService, Transform dragContainer)
        {
            _rectTransform.localScale = _defaultScaleVector;
            _cardFront.Init(_cardSO, _rectTransform, cardViewService);

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

        public void SetDragAndDropListener(ICardDragListener cardDragAndDropListener)
        {
            _cardDragAndDropActions.SetListener(cardDragAndDropListener);
        }

        public CardCharacter Play()
        {
            if (_cardCharacter == null)
            {
                CreateCardCharacter();
            }

            Deactivate();

            return _cardCharacter;
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
                    throw new ArgumentOutOfRangeException("SideType is not founded");
            }
        }

        public void SetActiveInteraction(bool isActive)
        {
            if (isActive)
            {
                if (_cardDragAndDrop.IsDragable == false)
                {
                    _cardSideFlipper.ActivateInteraction();
                }
            }
            else
            {
                _cardSideFlipper.DeactivateInteraction();
            }
        }

        public void DiscardCard()
        {
            Activate();
            
            _cardCharacter.Disable();
        }

        public Vector3 GetCardCharacterPosition()
        {
            if (_cardCharacter == null)
            {
                throw new NullReferenceException("СardCharacter is not instantiate");
            }

            return _cardCharacter.GetPosition();
        }

        private void CreateCardCharacter()
        {
            _cardCharacter = Instantiate(_cardSO.CardCharacter);
            _cardCharacter.Init(_cardSO.AwakeSound);
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

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
    }
}
