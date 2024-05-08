using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour, IUnbindCardManager, ICardDragListener, IBlockable
    {
        [SerializeField, Range(-1, 1)] private float _sortDirection;
        [SerializeField] private HandActions _handActions;

        public void Init(SeatPool seatPool)
        {
            _handActions.Init(_sortDirection, seatPool);
        }

        public void OnCardDrag(Card card)
        {
            _handActions.DragCard(card);
        }

        public void OnCardDrop()
        {
            _handActions.EndDragCard();
            _handActions.UnblockCards();
        }

        public void OnCardPlay()
        {
            _handActions.UnblockCards();
        }

        public void UnbindDragableCard()
        {
            _handActions.RemoveDraggableCard();
        }

        public virtual void AddCard(Card card)
        {
            _handActions.AddCard(card);
        }

        public bool TryGetCard(out Card card)
        {
            return _handActions.TryGetCard(out card);
        }

        public void Block()
        {
            _handActions.BlockCards();
            _handActions.EndDragCard();
        }

        public void Unblock()
        {
            _handActions.UnblockCards();
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
