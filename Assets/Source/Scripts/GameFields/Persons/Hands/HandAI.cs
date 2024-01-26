using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandAI : MonoBehaviour, IHand, ICardDragImitationListener
    {
        [SerializeField] private HandSeatList _handSeatList;
        [SerializeField] private CanvasGroup _canvasGroup;

        private const float SortDirection = -1;

        public void Init()
        {
            _handSeatList.Init(SortDirection);
            _canvasGroup.blocksRaycasts = true;
        }

        //public void CardDragAnimation()
        //{
        //    // NEW!
        //    // тут типо будет анимация лже-драга
        //    // замена метода OnCardDrag
        //    // ну или в отдельном классе
        //}

        //public void CardDropAnimation()
        //{
        //    // NEW!
        //    // тут типо будет анимация лже-дропа после драга
        //    // замена метода OnCardDrop
        //    // ну или в отдельном классе
        //}

        public void OnCardDrag(Card card)
        {
            _handSeatList.DragCard(card);
        }

        public void OnCardDrop()
        {
            _handSeatList.EndDragCard();
        }

        public bool TryGetCard(out Card card)
        {
            if (_handSeatList.TryGetCard(out card))
            {
                return true;
            }

            return false;
        }

        public void RemoveCard(Card card)
        {
            _handSeatList.RemoveCard();
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
