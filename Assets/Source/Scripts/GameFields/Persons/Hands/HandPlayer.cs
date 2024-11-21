using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandPlayer : Hand
    {
        public override void AddCard(Card card)
        {
            card.SetDragAndDropListener(this);

            base.AddCard(card);
        }
    }
}
