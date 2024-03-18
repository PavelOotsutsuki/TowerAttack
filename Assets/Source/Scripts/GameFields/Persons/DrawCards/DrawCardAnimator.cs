using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public abstract class DrawCardAnimator : MonoBehaviour
    {
        protected IHand Hand;
        protected Transform Parent;

        public void Init(IHand hand, Transform parent)
        {
            Hand = hand;
            Parent = parent;
        }

        public abstract IEnumerator PlayingSimpleDrawCardAnimation(Card drawnCard);
    }
}
