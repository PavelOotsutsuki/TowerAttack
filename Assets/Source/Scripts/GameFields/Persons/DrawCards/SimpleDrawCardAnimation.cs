using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class SimpleDrawCardAnimation : IDrawCardAnimation
    {
        private readonly Hand _hand;
        private readonly float _delay;

        public bool IsDone { get; private set; }

        public SimpleDrawCardAnimation(Hand hand, float delay)
        {
            _hand = hand;
            _delay = delay;

            IsDone = true;
        }

        public void Play(Card card)
        {
            Playing(card).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            IsDone = false;

            yield return new WaitForSeconds(_delay);

            _hand.AddCard(drawnCard);
            IsDone = true;
        }
    }
}