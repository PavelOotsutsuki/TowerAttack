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
        //private Action _startDrawCallback;

        public void Init(IHand hand)
        {
            //_hand = hand;
            _isDrawing = false;
            _drawCardQueue = new Queue<Card>();

            //_startDrawCallback = startDrawCallback;
            _drawCardAnimator.Init(hand);
        }

        //public void StartTurnDraw(Queue<Card> cards)
        //{
        //    AddCardsInQueue(cards);

        //    StartTurnDrawing().ToUniTask();
        //}

        public void TakeCards(Queue<Card> cards, Action callback = null)
        {
            AddCardsInQueue(cards);

            if (_isDrawing == false)
            {
                DrawingCards(callback).ToUniTask();
            }
        }

        //private IEnumerator StartTurnDrawing()
        //{
        //    yield return DrawingCards();

        //    _startDrawCallback?.Invoke();
        //}

        private IEnumerator DrawingCards(Action callback)
        {
            _isDrawing = true;

            while (_drawCardQueue.Count > 0)
            {
                Card card = _drawCardQueue.Dequeue();
                _drawCardAnimator.PlayingSimpleDrawCardAnimation(card);

                yield return new WaitUntil(() => _drawCardAnimator.IsDone);
            }

            callback?.Invoke();
            _isDrawing = false;
        }

        private void AddCardsInQueue(Queue<Card> cards)
        {
            while(cards.Count > 0)
            {
                _drawCardQueue.Enqueue(cards.Dequeue());
            }
        }
    }
}
