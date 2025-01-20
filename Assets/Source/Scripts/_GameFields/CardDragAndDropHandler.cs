using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.LightControls;
using UnityEngine;

namespace GameFields
{
    public class CardDragAndDropHandler : ICardDragAndDropHandler, ICardDragAndDropBlockable
    {
        private readonly ICardDragAndDropHandHandler _cardDragAndDropHandPlayer;
        private readonly IHandBlockable _handPlayerBlockable; 
        private readonly LightController _lightController;

        public CardDragAndDropHandler(ICardDragAndDropHandHandler cardDragAndDropHandPlayer, IHandBlockable handPlayerBlockable, LightController lightController)
        {
            _cardDragAndDropHandPlayer = cardDragAndDropHandPlayer;
            _handPlayerBlockable = handPlayerBlockable;
            _lightController = lightController;
        }

        public float ReturnInSeatDuration => _cardDragAndDropHandPlayer.ReturnInSeatDuration;

        public bool IsDraggable(Card card) => _cardDragAndDropHandPlayer.IsDraggable(card);

        public void OnCardAttack()
        {
            _cardDragAndDropHandPlayer.OnCardAttack();
            _lightController.Deactivate();
        }

        public void OnCardDrag(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardDrag(card);
            _lightController.Activate();
        }

        public void OnCardDrop()
        {
            _cardDragAndDropHandPlayer.OnCardDrop();
            _lightController.Deactivate();
        }

        public void OnCardPlay()
        {
            _cardDragAndDropHandPlayer.OnCardPlay();
            _lightController.Deactivate();
        }

        public void OnCardReturnInHand(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardReturnInHand(card);
        }

        public void ForciblyBlock()
        {
            _handPlayerBlockable.ForciblyBlock();
            _lightController.Deactivate();
        }

        public void Unblock()
        {
            _handPlayerBlockable.Unblock();
        }
    }
}