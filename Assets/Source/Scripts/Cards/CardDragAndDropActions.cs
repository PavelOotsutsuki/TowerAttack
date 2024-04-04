using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private IPlayable _playableCard;
        private IHandSeatable _handSeatableCard;
        private ICardDragListener _cardDragListener;

        internal CardDragAndDropActions(CardFront cardFront, IPlayable playableCard, IHandSeatable handSeatableCard)
        {
            _cardFront = cardFront;
            _playableCard = playableCard;
            _handSeatableCard = handSeatableCard;
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

            _cardDragListener.OnCardDrag(_handSeatableCard);

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

        internal bool TryDrop(ICardPlayPlace cardPlayPlace)
        {
            if (cardPlayPlace.TryPlayCard(_playableCard))
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
            _handSeatableCard.ReturnToHand(duration);
        }
    }
}
