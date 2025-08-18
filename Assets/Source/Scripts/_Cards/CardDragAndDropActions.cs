using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private readonly CardFront _cardFront;
        private readonly Card _card;
        private readonly ICardDragAndDropHandler _cardDragAndDropHandler;

        internal CardDragAndDropActions(CardFront cardFront, Card card, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            _cardFront = cardFront;
            _card = card;
            _cardDragAndDropHandler = cardDragAndDropHandler;
        }

        internal float ReturnInHandDuration => _cardDragAndDropHandler.ReturnInSeatDuration;
        internal bool CanDrag() => _cardDragAndDropHandler.IsDraggable(_card) && _cardFront.IsBlock == false;

        //internal void SetListener(ICardDragAndDropHandler cardDragAndDropHandler)
        //{
        //    _cardDragAndDropHandler = cardDragAndDropHandler;
        //}

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

            _cardDragAndDropHandler.OnCardDrag(_card);

            _cardFront.Block();
        }

        internal void OnReturnInHand(bool isPointerOnCard)
        {
            _cardDragAndDropHandler.OnCardReturnInHand(_card);

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
            _cardDragAndDropHandler.OnCardDrop();
        }

        internal void PlayCard(ICardDropPlace cardDropPlace)
        {
            _cardDragAndDropHandler.OnCardPlay();
            cardDropPlace.SeatCard(_card);
        }

        internal void Attack(IAttackable cardAttackZone)
        {
            cardAttackZone.Attack(_card);
            _cardDragAndDropHandler.OnCardPlay();
            //_cardDragAndDropHandler.OnCardAttack();
        }

        internal bool IsForgable()
        {
            return (_card.CardCapability & CardCapability.GnomeForging) == CardCapability.GnomeForging;
        }

        internal bool IsAttackable()
        {
            return (_card.CardCapability & CardCapability.Attack) == CardCapability.Attack;
        }

        internal bool IsPlayable()
        {
            return (_card.CardCapability & CardCapability.Play) == CardCapability.Play;
        }

        internal void StartForging(IForging forgingZone)
        {
            forgingZone.StartForging(_card);
            _cardDragAndDropHandler.OnCardPlay();
            //_cardDragAndDropHandler.OnCardAttack();
        }

        internal void ReturnInHand(float duration)
        {
            _card.CardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _card.DefaultScaleVector);
        }
    }
}