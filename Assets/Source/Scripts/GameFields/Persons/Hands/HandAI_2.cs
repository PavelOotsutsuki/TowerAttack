using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandAI_2 : Hand_2
    {
        private const float SortDirection = -1;

        public override void Init()
        {
            base.Init();
            CanvasGroup.blocksRaycasts = true;
        }

        //public void OnCardDrag(Card card)
        //{
        //    if (TryFindHandSeat(out HandSeat handSeat, card))
        //    {
        //        _dragCardHandSeat = handSeat;

        //        _canvasGroup.blocksRaycasts = false;
        //        _handSeatIndex = _handSeats.IndexOf(_dragCardHandSeat);
        //        _handSeats.Remove(_dragCardHandSeat);
        //        SortHandSeats();
        //    }
        //}
        public void CardDragAnimation() 
        {
            // NEW!
            // тут типо будет анимация лже-драга
            // замена метода OnCardDrag
            // ну или в отдельном классе
        }

        //public void OnCardDrop()
        //{
        //    _handSeats.Insert(_handSeatIndex, _dragCardHandSeat);
        //    OnEndCardDrag();
        //    SortHandSeats();
        //}
        public void CardDropAnimation()
        {
            // NEW!
            // тут типо будет анимация лже-дропа после драга
            // замена метода OnCardDrop
            // ну или в отдельном классе
        }

        protected override float GetSortDirection()
        {
            return SortDirection;
        }
    }
}
