using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandPlayer : Hand, IBlockable
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public override void OnCardDrag(Card card)
        {
            base.OnCardDrag(card);

            _canvasGroup.blocksRaycasts = false;
        }

        public override void OnCardDrop()
        {
            base.OnCardDrop();
            
            _canvasGroup.blocksRaycasts = true;
        }

        public override void UnbindDragableCard()
        {
            base.UnbindDragableCard();

            _canvasGroup.blocksRaycasts = true;
        }

        public override void AddCard(Card card)
        {
            //_canvasGroup.blocksRaycasts = false;

            card.SetDragAndDropListener(this);

            base.AddCard(card);
        }

        public void Block()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void Unblock()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
