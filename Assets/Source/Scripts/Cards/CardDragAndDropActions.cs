namespace Cards
{
    internal class CardDragAndDropActions
    {
        private readonly CardFront _cardFront;
        private readonly Card _card;
        private readonly CardMovement _movement;
        private readonly IBlockable _blockable;
        
        private ICardDragListener _cardDragListener;

        public CardDragAndDropActions(CardFront cardFront, Card card, CardMovement movement, IBlockable blockable)
        {
            _cardFront = cardFront;
            _card = card;
            _movement = movement;
            _blockable = blockable;
        }

        internal void SetListener(ICardDragListener cardDragListener)
        {
            _cardDragListener = cardDragListener;
        }

        internal void StartDrag()
        {
            if (_blockable.IsBlocked == false)
            {
                _cardFront.EndReview();
            }

            _cardDragListener.OnCardDrag(_card);

            _blockable.Block();
        }

        internal void OnReturnInHand(bool isPointerOnCard)
        {
            _blockable.Unblock();

            if (isPointerOnCard)
            {
                if (_blockable.IsBlocked == false)
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

        internal void ReturnInHand(float duration)
        {
            _movement.ReturnToHand(duration);
        }
    }
}
