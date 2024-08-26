using System.Collections;
using System.Collections.Generic;
using GameFields.Seats;
using UnityEngine;
using UnityEngine.EventSystems;
using Cards;
using Tools;

namespace GameFields.Persons.Hands
{
    public class HandSeat : Seat
    {
        [SerializeField] Transform _dragContainer;
        [SerializeField] DragAndDrop _dragAndDrop;

        private HandSeatDragAndDropActions _seatDragAndDropActions;

        public override void Init()
        {
            _seatDragAndDropActions = new HandSeatDragAndDropActions();//Card);

            _dragAndDrop.Init(transform, _seatDragAndDropActions, _dragContainer);
            base.Init();
        }
    }
}
