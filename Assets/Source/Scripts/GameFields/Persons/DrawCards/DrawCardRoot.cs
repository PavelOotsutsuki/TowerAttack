using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cards;

namespace GameFields.Persons.DrawCards
{
    public class DrawCardRoot
    {
        //[SerializeField] private int _countStartDrawCards = 1;

        //[SerializeField] private float _fireDrawCardsDelay = 2f;
        //[SerializeField] private float _simpleDrawCardsDelay = 0.1f;

        private SimpleDrawCardAnimation _simpleDrawCardAnimation;
        //private FireDrawCardAnimation _fireDrawCardAnimation; 
        //private StartDrawCardAnimator _startDrawCardAnimator;

        private IDrawCardAnimation _currentDrawCardAnimation;
        private Deck _deck;
        private bool _isDrawing;
        private readonly MonoBehaviour _coroutineContainer;

        public bool IsDrawing => _isDrawing;
        //private Action _startDrawCallback;

        public DrawCardRoot(SimpleDrawCardAnimation simpleDrawCardAnimation, Deck deck, MonoBehaviour coroutineContainer)
        {
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _deck = deck;
            _isDrawing = false;
            _coroutineContainer = coroutineContainer;
        }

        public List<Card> DrawCards(int countCards, Action callback = null)
        {
            _currentDrawCardAnimation = _simpleDrawCardAnimation;

            return TakeCards(countCards, callback);
        }

        public void DrawCards(IDrawCardAnimation drawCardAnimation, int countCards, Action callback = null)
        {
            _currentDrawCardAnimation = drawCardAnimation;

            TakeCards(countCards, callback);
        }

        private List<Card> TakeCards(int countCards, Action callback = null)
        {
                List<Card> cards = new List<Card>();

                for (int i = 0; i < countCards; i++)
                {
                    if (_deck.IsHasCards(1))
                    {
                        cards.Add(_deck.TakeTopCard());
                    }
                }

                _coroutineContainer.StartCoroutine(DrawingCards(cards, callback));

                return cards;
        }

        private IEnumerator DrawingCards(List<Card> cards, Action callback)
        {
            _isDrawing = true;

            if (cards.Count > 0)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    Card card = cards[i];
                    _currentDrawCardAnimation.Play(card);

                    yield return new WaitUntil(() => _currentDrawCardAnimation.IsDone);
                }
            }


            callback?.Invoke();

            _isDrawing = false;
        }

        //private void AddCardsInQueue(Queue<Card> cards)
        //{
        //    while(cards.Count > 0)
        //    {
        //        _drawCardQueue.Enqueue(cards.Dequeue());
        //    }
        //}
    }
}
