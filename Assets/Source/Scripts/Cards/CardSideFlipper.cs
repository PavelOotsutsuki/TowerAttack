using UnityEngine;

namespace Cards
{
     internal class CardSideFlipper
    {
        private CardFront _front;
        private CardBack _back;
        private CardDragAndDrop _cardDragAndDrop;

        public CardSideFlipper(CardFront front, CardBack back, CardDragAndDrop cardDragAndDrop)
        {
            _front = front;
            _back = back;
            _cardDragAndDrop = cardDragAndDrop;
        }

        public bool IsFrontSide { get; private set; }

        public void SetBackSide()
        {
            SetSide(true);
        }

        public void SetFrontSide()
        {
            SetSide(false);
        }

        public void DeactivateInteraction()
        {
            _cardDragAndDrop.Disable();
            _front.Block();
        }

        public void ActivateInteraction()
        {
            _cardDragAndDrop.Enable();
            _front.Unblock();
        }

        private void SetSide(bool isBackSide)
        {
            _front.gameObject.SetActive(isBackSide == false);
            _back.gameObject.SetActive(isBackSide);
            IsFrontSide = isBackSide == false;
        }
    }
}
