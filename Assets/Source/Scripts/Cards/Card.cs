using UnityEngine;
using Tools;

namespace Cards
{
    public class Card : MonoBehaviour, ICardProtectable
    {
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardMovement _cardMovement;
        [SerializeField] private Vector3 _defaultScaleVector;

        private DrawCardAnimation _drawCardAnimation;

        internal void Init(CardDescription cardDescription, BigCard bigCard, bool isBackView = true)
        {
            _rectTransform.localScale = _defaultScaleVector;
            _cardMovement = new CardMovement(_rectTransform);
            _cardSO.CardCharacter.Init(_cardSO.AwakeSound);
            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);
            _cardView = new CardView(_cardFront, _cardBack, isBackView);

            CardDragAndDropActions cardDragAndDropActions = new CardDragAndDropActions(_cardFront, _cardMovement);
            _cardDragAndDrop.Init(_rectTransform, cardDragAndDropActions);
            _drawCardAnimation = new DrawCardAnimation(_rectTransform, _cardView, _cardMovement, this);

            ActivateDragAndDrop(isBackView == false);
        }

        public void AddToTable(out CardCharacter cardCharacter)
        {
            cardCharacter = _cardSO.CardCharacter;
            Destroy();
        }

        public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardMovement.TranslateLocalSmoothly(positon, rotation, duration, _defaultScaleVector);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            Block();
            StartCoroutine(_drawCardAnimation.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent));
        }

        public void ActivateDragAndDrop(bool isActivate)
        {
            _cardDragAndDrop.enabled = isActivate;
        }

        public void Block()
        {
            _cardDragAndDrop.enabled = false;
            _cardFront.Block();
            Debug.Log("Block");
        }

        public void Unblock()
        {
            _cardDragAndDrop.enabled = true;
            _cardFront.Unblock();
            Debug.Log("Unblock");
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
            DefineCardMovement();
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

        [ContextMenu(nameof(DefineCardMovement))]
        private void DefineCardMovement()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardMovement, ComponentLocationTypes.InThis);
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
