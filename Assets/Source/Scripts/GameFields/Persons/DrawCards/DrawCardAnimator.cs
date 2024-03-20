using System.Collections;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public abstract class DrawCardAnimator : MonoBehaviour
    {
        protected IHand Hand;

        public abstract bool IsDone { get; }

        public void Init(IHand hand)
        {
            Hand = hand;
        }

        public abstract void PlayingSimpleDrawCardAnimation(Card drawnCard);
    }
}
