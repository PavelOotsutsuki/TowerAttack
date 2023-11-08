using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private CardMovement _cardMovement;

        internal CardDragAndDropActions(CardFront cardFront, CardMovement cardMovement)
        {
            _cardFront = cardFront;
            _cardMovement = cardMovement;
        }

        internal void EndReview()
        {
            if (_cardFront.IsBlock == false)
            {
                _cardFront.EndReview();
            }
        }

        internal void StartReview()
        {
            if (_cardFront.IsBlock == false)
            {
                _cardFront.StartReview();
            }
        }

        internal void BlockReview()
        {
            _cardFront.Block();
        }

        internal void UnblockReview()
        {
            _cardFront.Unblock();
        }

        internal void DisableRaycasts()
        {
            _cardFront.DisableRaycasts();
        }

        internal void EnableRaycasts()
        {
            _cardFront.EnableRaycasts();
        }

        internal void ReturnInHand(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardMovement.TranslateSmoothly(positon, rotation, duration);
        }
    }
}
