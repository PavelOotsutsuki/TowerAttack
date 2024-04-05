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

        public void OnCardDrag(Card card)
        {
            _handActions.DragCard(card);
        }

        public void OnCardDrop()
        {
            _handActions.EndDragCard();
        }

        public void UnbindDragableCard()
        {
            _handActions.RemoveDraggableCard();
        }

        public void AddCard(Card card)
        {
            _handActions.AddCard(card);
        }

        public bool TryGetCard(out Card card)
        {
            return _handActions.TryGetCard(out card);
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
