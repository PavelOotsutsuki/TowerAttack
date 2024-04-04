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
        private Queue<IHandSeatable> _drawCardQueue;
        private bool _isDrawing;
        private Action _startDrawCallback;

        public void Init(IHand hand, Action startDrawCallback)
        {
            //_hand = hand;
            _isDrawing = false;
            _drawCardQueue = new Queue<IHandSeatable>();

            _startDrawCallback = startDrawCallback;
            _drawCardAnimator.Init(hand);
        }

        public void StartTurnDraw(Queue<IHandSeatable> cards)
        {
            AddCardsInQueue(cards);

            StartTurnDrawing().ToUniTask();
        }

        public void TakeCards(Queue<IHandSeatable> cards)
        {
            AddCardsInQueue(cards);

            DrawingCards().ToUniTask();
        }

        private IEnumerator StartTurnDrawing()
        {
            yield return DrawingCards();

            _startDrawCallback?.Invoke();
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

        private IEnumerator DrawingCards()
        {
            if (_isDrawing)
            {
                yield break;
            }

            _isDrawing = true;

            while (_drawCardQueue.Count > 0)
            {
                IHandSeatable card = _drawCardQueue.Dequeue();
                _drawCardAnimator.PlayingSimpleDrawCardAnimation(card);
                yield return new WaitUntil(() => _drawCardAnimator.IsDone);
            }

            _isDrawing = false;
        }

        private void AddCardsInQueue(Queue<IHandSeatable> cards)
        {
            while(cards.Count > 0)
            {
                _drawCardQueue.Enqueue(cards.Dequeue());
            }
        }
    }
}
