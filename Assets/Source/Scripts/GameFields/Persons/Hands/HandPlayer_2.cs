using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandPlayer_2 : Hand_2, ICardDragListener
    {
        private const float SortDirection = 1;

        public void OnCardDrag(Card card)
        {
            if (TryFindHandSeat(out HandSeat handSeat, card))
            {
                DragCardHandSeat = handSeat;

                CanvasGroup.blocksRaycasts = false;
                HandSeatIndex = HandSeats.IndexOf(DragCardHandSeat);
                HandSeats.Remove(DragCardHandSeat);
                SortHandSeats();
            }
        }

        public void OnCardDrop()
        {
            HandSeats.Insert(HandSeatIndex, DragCardHandSeat);
            OnEndCardDrag();
            SortHandSeats();
        }

        public override void RemoveCard(Card card)
        {
            base.RemoveCard(card);
            OnEndCardDrag();
        }

        private void OnEndCardDrag()
        {
            CanvasGroup.blocksRaycasts = true;
            HandSeatIndex = -1;
            DragCardHandSeat = null;
        }

        protected override float GetSortDirection()
        {
            return SortDirection;
        }
    }
}
