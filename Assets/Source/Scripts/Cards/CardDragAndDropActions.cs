using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private CardMovement _cardMovement;
        private Card _card;
        private ICardDragListener _cardDragListener;

        internal CardDragAndDropActions(CardFront cardFront, CardMovement cardMovement, Card card)
        {
            _cardFront = cardFront;
            _cardMovement = cardMovement;
            _card = card;
        }

        internal void SetListener(ICardDragListener cardDragListener)
        {
            _cardDragListener = cardDragListener;
        }

        internal void StartDrag()
        {
            if (_cardFront.IsBlock == false)
            {
                _cardFront.EndReview();
            }

            _cardDragListener.OnCardDrag(_card);

            _cardFront.Block();
        }

        internal void OnReturnInHand(bool isPointerOnCard)
        {
            _cardFront.Unblock();

            if (isPointerOnCard)
            {
                if (_cardFront.IsBlock == false)
                {
                    _cardFront.StartReview();
                }
            }
        }

        internal void EndDrag()
        {
            _cardDragListener.OnCardDrop();
        }

        internal void ReturnInHand(Vector2 positon, Vector3 rotation, float duration)
        {
            Vector3 scaleVector = new Vector3(1f, 1f, 1f);
            _cardMovement.TranslateLocalSmoothly(positon, rotation, duration, scaleVector);
        }
    }
}
