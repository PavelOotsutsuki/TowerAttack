using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class SimpleDrawCardAnimation: IDrawCardAnimation
    {
        private readonly Hand _hand;
        private readonly float _delay;
        private readonly IDrawCardWatcher _drawCardWatcher;

        private bool _isComplete;

        public SimpleDrawCardAnimation(Hand hand, float delay)
        {
            _hand = hand;
            _delay = delay;
            _drawCardWatcher = hand;

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
            _drawCardWatcher.SetCard(drawnCard);
            _isComplete = true;
        }
    }
}