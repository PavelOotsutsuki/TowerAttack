using System.Collections;
using System.Collections.Generic;
using System;
using GameFields.Persons.Hands;
using UnityEngine;
using Cards;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class DrawCardRoot
    {
        //[SerializeField] private int _countDrawCards = 1;
        //[SerializeField] private int _countStartTowerCardSelectionDrawCards = 3;
        //[SerializeField] private int _countPatriarchCorallDrawDrawCards = 3;
        //[SerializeField] private float _drawCardsDelay = 2f;
        [SerializeField] private DrawCardAnimator _drawCardAnimator;

        //private IHand _hand;
        private Queue<Card> _drawCardQueue;
        private bool _isDrawing;

        public void Init(IHand hand)
        {
            //_hand = hand;
            _isDrawing = false;
            _drawCardQueue = new Queue<Card>();

            _drawCardAnimator.Init(hand);
        }

        public void TakeCards(Card[] cards)
        {
            AddCardsInQueue(cards);

            DrawCards().ToUniTask();
        }

        //public IEnumerator TakeCards(int count)
        //{
        //    //if (_deck.IsHasCards(count) == false)
        //    //{
        //    //    throw new ArgumentOutOfRangeException("Недостаточно карт в колоде");
        //    //}

        //    AddCardsInQueue(count);

        //    yield return DrawCards();
        //}

        //public IEnumerator TakeCards(int count)
        //{
        //    AddCardsInQueue(count);

        //    yield return DrawCards();
        //}

        private IEnumerator DrawCards()
        {
            if (_isDrawing)
            {
                yield break;
            }

            _isDrawing = true;

            while (_drawCardQueue.Count > 0)
            {
                Card card = _drawCardQueue.Dequeue();
                _drawCardAnimator.PlayingSimpleDrawCardAnimation(card);
                yield return new WaitUntil(() => _drawCardAnimator.IsDone);
            }

            _isDrawing = false;
        }

        private void AddCardsInQueue(Card[] cards)
        {
            foreach (Card card in cards)
            {
                _drawCardQueue.Enqueue(card);
            }
        }
    }
}
