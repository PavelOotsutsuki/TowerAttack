using System.Collections;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IDrawCardAnimation
    {
        private Hand _hand;
        private float _delay;
        private bool _isDone;
        private readonly MonoBehaviour _coroutineContainer;
        //private int _activeTurnsCount;

        public bool IsDone => _isDone;

        //public int ActiveTurnsCount => _activeTurnsCount;

        public FireDrawCardAnimation(Hand hand, float delay, MonoBehaviour coroutineContainer)
        {
            _hand = hand;
            _delay = delay;
            _coroutineContainer = coroutineContainer;
            //_activeTurnsCount = activeTurnsCount;

            _isDone = true;
        }

        public void Play(Card card)
        {
            _coroutineContainer.StartCoroutine(Playing(card));
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
