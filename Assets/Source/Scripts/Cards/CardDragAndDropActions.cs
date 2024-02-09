using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private Card _card;
        private ICardDragListener _cardDragListener;

        internal CardDragAndDropActions(CardFront cardFront, Card card)
        {
            _cardFront = cardFront;
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

        internal bool TryDrop(ICardDropPlace cardDropPlace)
        {
            if (cardDropPlace.TryGetCard(_card))
            {
                return true;
            }

            return false;
        }

        internal void EndDrag()
        {
            _cardDragListener.OnCardDrop();
        }

        internal void ReturnInHand(float duration)
        {
            _card.PlayReturnInHandAnimation( duration);
        }
    }
}
