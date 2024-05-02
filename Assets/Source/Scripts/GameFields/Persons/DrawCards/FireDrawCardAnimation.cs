using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class FireDrawCardAnimation : IExtraDrawCardAnimation
    {
        private Hand _hand;
        private float _delay;
        private bool _isDone;
        private int _activeTurnsCount;

        public bool IsDone => _isDone;

        public int ActiveTurnsCount => _activeTurnsCount;

        public FireDrawCardAnimation(Hand hand, float delay, int activeTurnsCount)
        {
            _hand = hand;
            _delay = delay;
            _activeTurnsCount = activeTurnsCount;

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
