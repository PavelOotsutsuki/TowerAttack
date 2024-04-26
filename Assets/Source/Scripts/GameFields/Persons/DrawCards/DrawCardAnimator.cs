using System;
using System.Collections;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public abstract class DrawCardAnimator : MonoBehaviour
    {
        protected Hand Hand;

        public bool IsDone { get; protected set; }

        public void Init(Hand hand)
        {
            Hand = hand;
            IsDone = true;
        }

        public abstract void PlayingSimpleDrawCardAnimation(Card drawnCard);
    }
}
