using UnityEngine;
using Tools;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        private DrawCardAnimation _drawCardAnimation;
        private CardDragAndDropActions _cardDragAndDropActions;
        private CardMovement _cardMovement;
        private CardSideFlipper _cardSideFlipper;

        internal void Init(CardDescription cardDescription, BigCard bigCard, Transform dragContainer)
        {
            _rectTransform.localScale = _defaultScaleVector;
            _cardMovement = new CardMovement(_rectTransform);
            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, _cardMovement, this);
            _cardDragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);
            _drawCardAnimation = new DrawCardAnimation(_rectTransform, _cardMovement, _cardSideFlipper);

            _cardSideFlipper = new CardSideFlipper(_cardFront, _cardBack, _cardDragAndDrop);
            _cardSideFlipper.SetBackSide();
        }

        public void BindSeat(Transform transform, bool isFrontSide, float duration = 0f)
        {
            _rectTransform.SetParent(transform);
            _cardMovement.TranslateLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _defaultScaleVector);
            //_cardMovement.TranslateLocalSmoothly(Vector2.zero, Vector3.zero, duration, _defaultScaleVector);

            if (isFrontSide)
            {
                _cardSideFlipper.SetFrontSide();
            }
            else
            {
                _cardSideFlipper.SetBackSide();
            }
        }

        public void SetEndDragListener(ICardDragListener cardDragListener)
        {
            _cardDragAndDropActions.SetListener(cardDragListener);
        }

        public void Play(out CardCharacter cardCharacter)
        {
            cardCharacter = Instantiate(_cardSO.CardCharacter);
            cardCharacter.Init(_cardSO.AwakeSound);
            Destroy();
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent, float screenFactor)
        {
            //Block();
            StartCoroutine(_drawCardAnimation.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent, screenFactor));
        }

        //private void Block()
        //{
        //    if (_cardSideFlipper.IsFrontSide)
        //    {
        //        return;
        //    }

        //    _cardDragAndDrop.enabled = false;
        //    _cardFront.Block();
        //}

        //private void Unblock()
        //{
        //    if (_cardSideFlipper.IsFrontSide == false)
        //    {
        //        return;
        //    }

        //    _cardDragAndDrop.enabled = true;
        //    _cardFront.Unblock();
        //}

        private void Destroy()
        {
            Destroy(gameObject);
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
