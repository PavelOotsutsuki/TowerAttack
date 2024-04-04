using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandPlayer : MonoBehaviour, IHand, ICardDragListener
    {
        private const float SortDirection = 1;

        [SerializeField] private HandActions _handActions;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Init()
        {
            _handActions.Init(SortDirection);
        }

        public void OnCardDrag(ISeatable card)
        {
            _handActions.DragCard(card);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnCardDrop()
        {
            _handActions.EndDragCard();
            _canvasGroup.blocksRaycasts = true;
        }

        public void UnbindDragableCard()
        {
            _handActions.RemoveDraggableCard();
            _canvasGroup.blocksRaycasts = true;
        }

        public void AddCard(ISeatable seatableCard)
        {
            seatableCard.SetDragAndDropListener(this);
            _handActions.AddCard(seatableCard);
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
