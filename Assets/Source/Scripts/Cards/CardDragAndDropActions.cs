using System.Collections;
using Cysharp.Threading.Tasks;
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
        private ICardTransformable _blocker;

        internal CardDragAndDropActions(CardFront cardFront, Card card)
        {
            _cardFront = cardFront;
            _card = card;
            _movement = new Movement(_card.Transform);
            _blocker = _card;
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
            _blocker.SetActiveInteraction(true);

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

        //internal void UnblockCard()
        //{
        //    _cardFront.Unblock();
        //}
    }
}
