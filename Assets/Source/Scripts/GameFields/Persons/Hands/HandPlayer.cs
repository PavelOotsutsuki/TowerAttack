using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandPlayer : MonoBehaviour, IHand, ICardDragListener
    {
        private const float SortDirection = 1;

        [SerializeField] private HandSeatList _handSeatList;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Init()
        {
            _handSeatList.Init(SortDirection);
        }

        public void OnCardDrag(Card card)
        {
            _handSeatList.DragCard(card);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnCardDrop()
        {
            _handSeatList.EndDragCard();
            _canvasGroup.blocksRaycasts = true;
        }

        public void RemoveCard(Card card)
        {
            _handSeatList.RemoveCard();
            _canvasGroup.blocksRaycasts = true;
        }

        public void AddCard(Card card)
        {
            _handSeatList.AddCard(card);
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
            AutomaticFillComponents.DefineComponent(this, ref _handSeatList, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
