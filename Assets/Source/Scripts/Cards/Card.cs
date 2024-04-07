using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class Card : MonoBehaviour, ICardTransformable
    {
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        //private CardAnimator _cardAnimator;
        private CardDragAndDropActions _cardDragAndDropActions;
        //private CardMovement _cardMovement;
        private CardSideFlipper _cardSideFlipper;
        private CardCharacter _cardCharacter;
        //private ViewType _viewType;

        public RectTransform Transform => _rectTransform;
        public Vector3 DefaultScaleVector => _defaultScaleVector;

        internal void Init(CardDescription cardDescription, BigCard bigCard, Transform dragContainer)
        {
            _rectTransform.localScale = _defaultScaleVector;

            //_cardMovement = new CardMovement(_rectTransform, _defaultScaleVector);

            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, this);
            _cardDragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);

            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop);
            _cardSideFlipper.SetBackSide();

            //_cardAnimator = new CardAnimator(_rectTransform, _cardMovement, _cardSideFlipper);
            //_viewType = ViewType.Unselect;
        }

        //public void BindSeat(Transform transform, bool isFrontSide, float duration)
        //{
        //    _rectTransform.SetParent(transform);
        //    _cardMovement.BindSeatMovement(duration);

        //    if (isFrontSide)
        //    {
        //        SetEnableFrontSide();
        //    }
        //    else
        //    {
        //        SetBackSide();
        //    }
        //}

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
            switch(sideType)
            {
                case SideType.EnableFront:
                    SetEnableFrontSide();
                    break;
                case SideType.DisableFront:
                    SetDisableFrontSide();
                    break;
                case SideType.Back:
                    SetBackSide();
                    break;
                default:
                    throw new NullReferenceException("SideType is not founded");
            }
        }

        //public void Drawn(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        //{
        //    _cardAnimator.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent);
        //}

        //public void PlayOnPlace(Vector3 center, float duration)
        //{
        //    _cardMovement.MoveOnPlace(center, duration);
        //}

        //public void ReturnToHand(float duration)
        //{
        //    _cardMovement.MoveReturnToHand(duration);
        //}

        //public void ViewCard(float duration)
        //{
        //    ChangeViewType();

        //    _cardMovement.ViewCardMovement(_viewType, duration);
        //}

        public void DiscardCard()
        {
            if (gameObject.activeInHierarchy == true)
            {
                return;
            }

            Activate();
            //Vector3 startPosition = _cardCharacter.GetPositon();
            _cardCharacter.Disable();
            //_cardAnimator.PlayDiscardCardAnimation(startPosition, discardCardAnimationData);
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

        private void SetDisableFrontSide()
        {
            _cardSideFlipper.SetFrontSide();
            _cardSideFlipper.Block();
        }

        private void SetEnableFrontSide()
        {
            _cardSideFlipper.SetFrontSide();
            _cardSideFlipper.Unblock();
        }

        private void SetBackSide()
        {
            _cardSideFlipper.SetBackSide();
            _cardSideFlipper.Block();
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        //private void ChangeViewType()
        //{
        //    switch (_viewType)
        //    {
        //        case ViewType.Select:
        //            _viewType = ViewType.Unselect;
        //            break;
        //        case ViewType.Unselect:
        //            _viewType = ViewType.Select;
        //            break;
        //        default:
        //            Debug.LogError("Unknown ViewType");
        //            break;
        //    }
        //}

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
