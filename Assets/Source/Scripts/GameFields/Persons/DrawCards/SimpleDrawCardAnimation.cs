using System.Collections;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class SimpleDrawCardAnimation: IDrawCardAnimation
    {
        private Hand _hand;
        private float _delay;
        private bool _isDone;
        private readonly MonoBehaviour _coroutineContainer;

        public bool IsDone => _isDone;

        public SimpleDrawCardAnimation(Hand hand, float delay, MonoBehaviour coroutineContainer)
        {
            _hand = hand;
            _delay = delay;
            _coroutineContainer = coroutineContainer;

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
            //_dfsdfsdf.SetListenter(_hand);
            _isDone = true;
        }
    }
}
