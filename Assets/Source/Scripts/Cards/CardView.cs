using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Tools;
using DG.Tweening;

namespace Cards
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;

        private RectTransform _cardRectTransform;
        private bool _isBackView;

        //public CardFront CardFront => _cardFront;

        public void Init(CardSO cardSO, RectTransform cartRectTransform, CardDescription cardDescription, BigCard bigCard, bool isBackView)
        {
            _cardRectTransform = cartRectTransform;
            _cardFront.Init(cardSO, _cardRectTransform, cardDescription, bigCard);
            _cardDragAndDrop.Init(_cardRectTransform, _cardFront);
            _cardBack.Init();
            _isBackView = isBackView;
            SetSideView(); 
        }

        private void SetSideView()
        {
            _cardBack.gameObject.SetActive(_isBackView);
            _cardFront.gameObject.SetActive(_isBackView == false);
        }

        public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardRectTransform.DOLocalMove(positon, duration);
            _cardRectTransform.DORotate(rotation, duration);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardFront();
            DefineCardBack();
            DefineCardDragAndDrop();
        }

        [ContextMenu(nameof(DefineCardFront))]
        private void DefineCardFront()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardFront, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardBack))]
        private void DefineCardBack()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardBack, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardDragAndDrop))]
        private void DefineCardDragAndDrop()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InChildren);
        }
    }
}
