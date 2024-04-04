using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandAI : MonoBehaviour, IHand, ICardDragImitationListener
    {
        [SerializeField] private HandActions _handActions;
        [SerializeField] private CanvasGroup _canvasGroup;

        private const float SortDirection = -1;

        public void Init()
        {
            _handActions.Init(SortDirection);
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnCardDrag(ISeatable seatableCard)
        {
            _handActions.DragCard(seatableCard);
        }

        public void OnCardDrop()
        {
            _handActions.EndDragCard();
        }

        public void UnbindDragableCard()
        {
            _handActions.RemoveDraggableCard();
        }

        public void AddCard(IHandSeatable seatableCard)
        {
            _handActions.AddCard(seatableCard);
        }

        public bool TryGetCard(out ISeatable seatableCard)
        {
            return _handActions.TryGetCard(out seatableCard);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHandSeatList();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineHandSeatList))]
        private void DefineHandSeatList()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handActions, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
