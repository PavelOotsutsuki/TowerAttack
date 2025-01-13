using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.LightControls;
using UnityEngine;

namespace GameFields
{
    public class CardDragAndDropHandler : ICardDragAndDropHandler
    {
        private readonly ICardDragAndDropHandHandler _handPlayer;
        private readonly LightController _lightController;

        public CardDragAndDropHandler(ICardDragAndDropHandHandler handPlayer, LightController lightController)
        {
            _handPlayer = handPlayer;
            _lightController = lightController;
        }

        public float ReturnInSeatDuration => _handPlayer.ReturnInSeatDuration;

        public bool IsDraggable(Card card) => _handPlayer.IsDraggable(card);

        public void OnCardAttack()
        {
            _handPlayer.OnCardAttack();
            _lightController.Deactivate();
        }

        public void OnCardDrag(Card card)
        {
            _handPlayer.OnCardDrag(card);
            _lightController.Activate();
        }

        public void OnCardDrop()
        {
            _handPlayer.OnCardDrop();
            _lightController.Deactivate();
        }

        public void OnCardPlay()
        {
            _handPlayer.OnCardPlay();
            _lightController.Deactivate();
        }

        public void OnCardReturnInHand(Card card)
        {
            _handPlayer.OnCardReturnInHand(card);
            //_lightController.Deactivate();
        }
    }
}
