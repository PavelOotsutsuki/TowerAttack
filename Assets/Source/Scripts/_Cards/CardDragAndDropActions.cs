using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private readonly CardFront _cardFront;
        private readonly Card _card;

        private ICardDragAndDropListener _cardDragAndDropListener;

        internal CardDragAndDropActions(CardFront cardFront, Card card)
        {
            _cardFront = cardFront;
            _card = card;
        }

        internal float ReturnInHandSpeed => _cardDragAndDropListener.ReturnInSeatDuration;
        internal bool CanDrag() => _cardDragAndDropListener.IsDraggable;

        internal void SetListener(ICardDragAndDropListener cardDragAndDropListener)
        {
            _cardDragAndDropListener = cardDragAndDropListener;
        }

        internal void StartDrag()
        {
            if (_cardFront.IsBlock == false)
            {
                _cardFront.EndReview();
            }
            else
            {
                Debug.LogWarning("StartDrag when _cardFront.IsBlock");
            }

            _cardDragAndDropListener.OnCardDrag(_card);

            _cardFront.Block();
        }

        internal void OnReturnInHand(bool isPointerOnCard)
        {
            _cardDragAndDropListener.OnCardReturnInHand(_card);

            if (isPointerOnCard && _cardFront.IsBlock == false)
            {
                _cardFront.StartReview();
            }
        }

        internal bool CanDrop(ICardDropPlace cardDropPlace)
        {
            return cardDropPlace.HasFreeSeat;
        }

        internal void StartEndDrag()
        {
            _cardDragAndDropListener.OnCardDrop();
        }

        internal void PlayCard(ICardDropPlace cardDropPlace)
        {
            _cardDragAndDropListener.OnCardPlay();
            cardDropPlace.SeatCard(_card);
        }

        internal void ReturnInHand(float duration)
        {
            _card.CardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _card.DefaultScaleVector);
        }
    }
}