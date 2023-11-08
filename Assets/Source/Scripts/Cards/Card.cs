using UnityEngine;
using Tools;

namespace Cards
{
    public class Card : MonoBehaviour, IDragAndDropable, ICardProtectable
    {
        [SerializeField] private CardSO _cardSO;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardMovement _cardMovement;
        [SerializeField] private float _translateSpeed;

        internal void Init(CardDescription cardDescription, BigCard bigCard, bool isBackView = true)
        {
            _cardMovement.Init(_rectTransform, this);
            _cardSO.CardCharacter.Init(_cardSO.AwakeSound);
            _cardView.Init(_cardSO, _rectTransform, cardDescription, bigCard, _cardMovement, this, isBackView);

            CardDragAndDropActions cardDragAndDropActions = new CardDragAndDropActions(_cardView.CardFront, _cardMovement);
            _cardDragAndDrop.Init(_rectTransform, cardDragAndDropActions);
        }

        public void AddToTable(out CardCharacter cardCharacter)
        {
            cardCharacter = _cardSO.CardCharacter;
            Destroy();
        }

        public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardMovement.TranslateLocalSmoothly(positon, rotation, duration);
        }

        public void PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            Block();
            StartCoroutine(_cardView.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent));
        }

        public void ActivateDragAndDrop(bool isActivate)
        {
            _cardDragAndDrop.enabled = isActivate;
        }

        public void Block()
        {
            _cardDragAndDrop.enabled = false;
            _cardView.CardFront.Block();
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
