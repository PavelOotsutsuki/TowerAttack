using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class Card : MonoBehaviour, IHandSeatable, ISeatable, IPlayable
    {
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        private CardAnimator _cardAnimator;
        private CardDragAndDropActions _cardDragAndDropActions;
        private CardMovement _cardMovement;
        private CardSideFlipper _cardSideFlipper;
        private CardCharacter _cardCharacter;
        private ViewType _viewType;

        public CardMovement CardMovement => _cardMovement;

        internal void Init(CardDescription cardDescription, BigCard bigCard, Transform dragContainer)
        {
            _rectTransform.localScale = _defaultScaleVector;

            _cardMovement = new CardMovement(_rectTransform, _defaultScaleVector);

            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, this, this);
            _cardDragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);

            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop);
            _cardSideFlipper.SetBackSide();

            _cardAnimator = new CardAnimator(_rectTransform, _cardMovement, _cardSideFlipper);
            _viewType = ViewType.Unselect;
        }

        public void BindSeat(Transform transform, bool isFrontSide, float duration = 0f)
        {
            _rectTransform.SetParent(transform);
            _cardMovement.BindSeatMovement(duration);

            if (isFrontSide)
            {
                _cardSideFlipper.SetFrontSide();
            }
            else
            {
                _cardSideFlipper.SetBackSide();
            }
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

        public void FlipOnPlayerDraw()
        {
            _cardSideFlipper.SetFrontSide();
            _cardSideFlipper.Block();
        }

        //public void Drawn(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        //{
        //    _cardAnimator.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent);
        //}

        public void PlayOnPlace(Vector3 center, float duration)
        {
            _cardMovement.MoveOnPlace(center, duration);
        }

        public void ReturnToHand(float duration)
        {
            _cardMovement.MoveReturnToHand(duration);
        }

        public void ViewCard(float duration)
        {
            ChangeViewType();

            _cardMovement.ViewCardMovement(_viewType, duration);
        }

        public void DiscardCard(DiscardCardAnimationData discardCardAnimationData)
        {
            Activate();
            Vector3 startPosition = _cardCharacter.DiscardCard();
            _cardAnimator.PlayDiscardCardAnimation(startPosition, discardCardAnimationData);
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

        private void ChangeViewType()
        {
            switch (_viewType)
            {
                case ViewType.Select:
                    _viewType = ViewType.Unselect;
                    break;
                case ViewType.Unselect:
                    _viewType = ViewType.Select;
                    break;
                default:
                    Debug.LogError("Unknown ViewType");
                    break;
            }
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
