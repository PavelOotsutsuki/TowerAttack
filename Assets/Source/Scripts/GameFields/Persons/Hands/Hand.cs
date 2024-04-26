using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour, IUnbindCardManager, ICardDragListener
    {
        [SerializeField, Range(-1, 1)] private float _sortDirection;
        [SerializeField] private HandActions _handActions;

        public void Init(SeatPool seatPool)
        {
            _handActions.Init(_sortDirection, seatPool);
        }

        public virtual void OnCardDrag(Card card)
        {
            _handActions.DragCard(card);
        }

        public virtual void OnCardDrop()
        {
            _handActions.EndDragCard();
        }

        public virtual void UnbindDragableCard()
        {
            _handActions.RemoveDraggableCard();
        }

        public virtual void AddCard(Card card)
        {
            _handActions.AddCard(card);
        }

        public virtual bool TryGetCard(out Card card)
        {
            return _handActions.TryGetCard(out card);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHandSeatList();
        }

        [ContextMenu(nameof(DefineHandSeatList))]
        private void DefineHandSeatList()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handActions, ComponentLocationTypes.InThis);
        }
    }
}
