using System;
using UnityEngine;
using Tools;

namespace Cards
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private CardBack _cardBack;

        private CardFront _cardFront;
        private RectTransform _cardRectTransform;

        public CardFront CardFront => _cardFront;

        public void Init(CardFront cardFront, bool isBackView)
        {
            _cardFront = cardFront;
            SetSideView(isBackView);
        }

        public void SetFrontSide()
        {
            SetSideView(false);
        }
        
        private void SetSideView(bool isBackView)
        {
            _cardBack.gameObject.SetActive(isBackView);
            _cardFront.gameObject.SetActive(isBackView == false);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardFront();
            DefineCardBack();
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
    }
}
