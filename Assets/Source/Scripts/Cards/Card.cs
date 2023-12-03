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
            //bool isBackView = true;
            _rectTransform.localScale = _defaultScaleVector;
            _cardMovement = new CardMovement(_rectTransform);
            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);
            _cardSideFlipper = new CardSideFlipper(_cardFront.gameObject, _cardBack.gameObject);
            _cardSideFlipper.SetBackSide();

            _cardDragAndDropActions = new CardDragAndDropActions(_cardFront, _cardMovement, this);
            _cardDragAndDrop.Init(_rectTransform, _cardDragAndDropActions, dragContainer);
            _drawCardAnimation = new DrawCardAnimation(_rectTransform, _cardMovement, _cardSideFlipper);

            Block();
        }

        public void BindSeat(Transform transform, float duration)
        {
            _rectTransform.SetParent(transform);
            _cardMovement.TranslateLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _defaultScaleVector);
            //_cardMovement.TranslateLocalSmoothly(Vector2.zero, Vector3.zero, duration, _defaultScaleVector);
            Unblock();
        }

        public void AddToHand(ICardDragListener cardDragListener)
        {
            _cardDragAndDropActions.SetListener(cardDragListener);
        }

        public void Play(out CardCharacter cardCharacter)
        {
            cardCharacter = Instantiate(_cardSO.CardCharacter);
            cardCharacter.Init(_cardSO.AwakeSound);
            Destroy();
        }

        //public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    _cardMovement.TranslateLocalSmoothly(positon, rotation, duration, _defaultScaleVector);
        //}

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            Block();
            StartCoroutine(_drawCardAnimation.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent));
        }

        //public void ActivateDragAndDrop(bool isActivate)
        //{
        //    _cardDragAndDrop.enabled = isActivate;
        //}

        //public void BlockDragAndDrop()
        //{

        //}

        private void Block()
        {
            _cardDragAndDrop.enabled = false;
            _cardFront.Block();
        }

        private void Unblock()
        {
            _cardDragAndDrop.enabled = true;
            _cardFront.Unblock();
        }

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
