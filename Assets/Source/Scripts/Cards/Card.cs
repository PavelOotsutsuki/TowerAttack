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
        [SerializeField] private CardDragAndDrop _dragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        private CardDragAndDropActions _cardDragAndDropActions;
        private CardSideFlipper _cardSideFlipper;
        private CardCharacter _cardCharacter;

        public RectTransform Transform => _rectTransform;
        public Vector3 DefaultScaleVector => _defaultScaleVector;

        internal void Init(CardViewService cardViewService, Transform dragContainer)
        {
            _rectTransform.localScale = _defaultScaleVector;

            _cardFront.Init(_cardSO, _rectTransform, cardViewService);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, this);
            _dragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);

            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _dragAndDrop);

            SetSide(DefaultSide);
            SetActiveInteraction(DefaultInteractionActive);
        }

        public void BindDragAndDropContainer(Transform container)
        {
            _dragAndDrop.BindParent(container);
        }

        public void ForceEndDrag()
        {
            _dragAndDrop.ForceBlockDrag();
        }

        public void SetDragAndDropListener(ICardDragListener cardDragAndDropListener)
        {
            _cardDragAndDropActions.SetListener(cardDragAndDropListener);
        }

        public CardCharacter PlayOnTable()
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
                if (_dragAndDrop.InDrag == false)
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
            _cardCharacter.Init(_cardSO.AwakeSound, _cardSO.Effect);
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
            AutomaticFillComponents.DefineComponent(this, ref _dragAndDrop, ComponentLocationTypes.InThis);
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
