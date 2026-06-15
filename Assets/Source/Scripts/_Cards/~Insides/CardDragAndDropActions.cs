using System.Threading;
using Cards.DependencyInterlayers;
using Cards.Views;
using UnityEngine;

namespace Cards.Insides
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

        internal void StartDrag()
        {
            if (_cardFront.IsBlock == false)
            {
                _cardFront.EndReview();
            }
            else
            {
                Debug.Log("StartDrag when _cardFront.IsBlock");
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

        internal bool CanDrop(ITableDrop tableDrop)
        {
            return tableDrop.HasFreeSeat && tableDrop.CanPlay;
        }

        internal void StartEndDrag()
        {
            _cardDragAndDropHandler.OnCardDrop();
        }

        internal void PlayCard()
        {
            _cardDragAndDropHandler.OnCardPlay();
            _card.Play();
        }

        internal void Attack(IAttackable cardAttackZone)
        {
            cardAttackZone.Attack(_card);
            _cardDragAndDropHandler.OnCardPlay();
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

        internal bool IsHandTransferable()
        {
            return (_card.CardCapability & CardCapability.HandTransfer) == CardCapability.HandTransfer;
        }

        internal void StartForging(IForging forgingZone)
        {
            forgingZone.StartExtraEffect(_card);
            _cardDragAndDropHandler.OnCardPlay();
        }

        internal void StartHandTransfing(IHandTransferable handTransferZone)
        {
            handTransferZone.StartExtraEffect(_card);
            _cardDragAndDropHandler.OnCardPlay();
        }

        internal void ReturnInHand(float duration)
        {
            _card.CardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, _card.DefaultScaleVector);
        }
    }
}