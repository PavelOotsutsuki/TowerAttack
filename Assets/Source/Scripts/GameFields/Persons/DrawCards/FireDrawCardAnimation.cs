using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private Hand _hand;
        private float _delay;
        private bool _isDone;

        public bool IsDone => _isDone;

        public FireDrawCardAnimation(Hand hand, float delay)
        {
            _hand = hand;
            _delay = delay;

            _isDone = true;
        }

        public void Play(Card card)
        {
            Playing(card).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            _isDone = false;

            yield return new WaitForSeconds(_delay);

            _hand.AddCard(drawnCard);
            _isDone = true;
        }
    }
}
