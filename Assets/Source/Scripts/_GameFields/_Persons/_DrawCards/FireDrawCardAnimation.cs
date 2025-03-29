using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private readonly Hand _hand;
        private readonly float _delay;

        private bool _isComplete;

        public FireDrawCardAnimation(Hand hand, float delay)
        {
            _hand = hand;
            _delay = delay;

            _isComplete = true;
        }

        public bool IsComplete => _isComplete;

        public void Play(Card card)
        {
            Playing(card).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            _isComplete = false;

            yield return new WaitForSeconds(_delay);

            _hand.AddCard(drawnCard);
            _isComplete = true;
        }
    }
}