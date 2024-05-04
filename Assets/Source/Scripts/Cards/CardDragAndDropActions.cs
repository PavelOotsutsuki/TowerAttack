using Tools;
using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private Card _card;
        private Movement _movement;
        private ICardDragListener _cardDragListener;

        internal CardDragAndDropActions(CardFront cardFront, Card card)
        {
            _cardFront = cardFront;
            _card = card;
            _movement = new Movement(_card.Transform);
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
            if (cardDropPlace.TrySeatCard(_card))
            {
                return true;
            }

            return false;
        }

        internal void EndDrag()
        {
            _cardDragListener.OnCardDrop();
        }

        internal void PlayCard()
        {
            _cardDragListener.OnCardPlay();
        }

        internal void ReturnInHand(float duration)
        {
            _movement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _card.DefaultScaleVector);
        }
    }
}
