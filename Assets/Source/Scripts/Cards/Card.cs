using UnityEngine;
using Tools;

namespace Cards
{
    public class Card : MonoBehaviour, IDragAndDropable, ICardProtectable
    {
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardMovement _cardMovement;

        private DrawCardAnimation _drawCardAnimation;
        
        internal void Init(CardDescription cardDescription, BigCard bigCard, bool isBackView = true)
        {
            _cardMovement.Init(_rectTransform, this);
            _cardSO.CardCharacter.Init(_cardSO.AwakeSound);
            _cardFront.Init(_cardSO, _rectTransform, cardDescription, bigCard);
            _cardView.Init(_cardFront, isBackView);

            CardDragAndDropActions cardDragAndDropActions = new(_cardView.CardFront, _cardMovement);
            _cardDragAndDrop.Init(_rectTransform, cardDragAndDropActions);

            _drawCardAnimation = new DrawCardAnimation(_cardView, _rectTransform, _cardMovement);
            
            ActivateDragAndDrop(isBackView == false);
        }

        public void AddToTable(out CardCharacter cardCharacter)
        {
            cardCharacter = _cardSO.CardCharacter;
            Destroy();
        }

        public void TranslateLocalInto(Vector2 position, Vector3 rotation, float duration)
        {
            _cardMovement.TranslateLocalSmoothly(position, rotation, duration);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            Block();
            
            StartCoroutine(_drawCardAnimation.Play(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent));
            _drawCardAnimation.Finish += OnDrawCardAnimationFinish;
        }

        private void OnDrawCardAnimationFinish()
        {
            _drawCardAnimation.Finish -= OnDrawCardAnimationFinish;
            
            Unblock();
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
            _cardView.CardFront.Unblock();
            Debug.Log("Unblock");
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
        
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardDragAndDrop();
            DefineCardView();
            DefineRectTransform();
            DefineCardMovement();
        }

        [ContextMenu(nameof(DefineCardDragAndDrop))]
        private void DefineCardDragAndDrop()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardView))]
        private void DefineCardView()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardView, ComponentLocationTypes.InThis);
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

    }
}
