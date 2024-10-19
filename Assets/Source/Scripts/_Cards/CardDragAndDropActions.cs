using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private readonly CardFront _cardFront;
        private readonly Card _card;

        private ICardDragAndDropListener _cardDragListener;

        internal CardDragAndDropActions(CardFront cardFront, Card card)
        {
            _cardFront = cardFront;
            _card = card;
        }

        internal void SetListener(ICardDragAndDropListener cardDragListener)
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
            _cardDragListener.OnCardReturnInHand(_card);

            if (isPointerOnCard && _cardFront.IsBlock == false)
            {
                _cardFront.StartReview();
            }
        }

        internal bool IsCanDrop(ICardDropPlace cardDropPlace)
        {
            return cardDropPlace.HasFreeSeat;
        }

        internal void StartEndDrag()
        {
            _cardDragListener.OnCardDrop();
        }

        internal void PlayCard(ICardDropPlace cardDropPlace)
        {
            _cardDragListener.OnCardPlay();
            cardDropPlace.SeatCard(_card);
        }

        internal void ReturnInHand(float duration)
        {
            _card.CardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _card.DefaultScaleVector);
        }

        internal bool IsCanDrag()
        {
            return _cardDragListener.IsDraggable;
        }
    }
}
