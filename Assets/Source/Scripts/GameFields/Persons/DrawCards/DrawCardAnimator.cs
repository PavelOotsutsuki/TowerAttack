using System;
using System.Collections;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public abstract class DrawCardAnimator : MonoBehaviour
    {
        protected IHand Hand;

        public bool IsDone { get; protected set; }

        public void Init(IHand hand)
        {
            Hand = hand;
            IsDone = true;
        }

        public abstract void PlayingSimpleDrawCardAnimation(Card drawnCard);
    }
}
