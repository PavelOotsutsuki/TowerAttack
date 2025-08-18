using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.LightControls;
using UnityEngine;
using CanvasSortOrders;

namespace GameFields
{
    public class CardDragAndDropHandler : ICardDragAndDropHandler, ICardDragAndDropBlockable
    {
        private readonly ICardDragAndDropHandHandler _cardDragAndDropHandPlayer;
        private readonly IHandBlockable _handPlayerBlockable; 
        private readonly LightController _lightController;
        private readonly CanvasSortOrder _sortOrder;

        public CardDragAndDropHandler(ICardDragAndDropHandHandler cardDragAndDropHandPlayer, IHandBlockable handPlayerBlockable,
            LightController lightController, SpeedUpButtonSortOrder sortOrder)
        {
            _cardDragAndDropHandPlayer = cardDragAndDropHandPlayer;
            _handPlayerBlockable = handPlayerBlockable;
            _lightController = lightController;
            _sortOrder = sortOrder;
        }

        public float ReturnInSeatDuration => _cardDragAndDropHandPlayer.ReturnInSeatDuration;

        public bool IsDraggable(Card card) => _cardDragAndDropHandPlayer.IsDraggable(card);

        public void OnCardAttack()
        {
            _cardDragAndDropHandPlayer.OnCardAttack();
            _lightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardDrag(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardDrag(card);
            _lightController.Activate();
            _sortOrder.SetSortIndex(-1);
        }

        public void OnCardDrop()
        {
            _cardDragAndDropHandPlayer.OnCardDrop();
            _lightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardPlay()
        {
            _cardDragAndDropHandPlayer.OnCardPlay();
            _lightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardReturnInHand(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardReturnInHand(card);
        }

        public void ForciblyBlock()
        {
            _handPlayerBlockable.ForciblyBlock();
            _lightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void Unblock()
        {
            _handPlayerBlockable.Unblock();
        }
    }
}