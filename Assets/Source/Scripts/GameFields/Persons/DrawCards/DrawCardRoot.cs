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
        //[SerializeField] private int _countStartTowerCardSelectionDrawCards = 3;
        //[SerializeField] private int _countPatriarchCorallDrawDrawCards = 3;
        [SerializeField] private int _countStartDrawCards = 1;

        [SerializeField] private float _fireDrawCardsDelay = 2f;
        [SerializeField] private float _simpleDrawCardsDelay = 0.1f;

        private SimpleDrawCardAnimation _simpleDrawCardAnimation;
        private FireDrawCardAnimation _fireDrawCardAnimation; 
        private StartDrawCardAnimator _startDrawCardAnimator;

        private IDrawCardAnimation _currentDrawCardAnimation;
        private Deck _deck;
        private bool _isDrawing;

        public bool IsDrawing => _isDrawing;
        //private Action _startDrawCallback;

        public void Init(Hand hand, Deck deck)
        {
            _deck = deck;
            _isDrawing = false;

            //_startDrawCallback = startDrawCallback;
            _simpleDrawCardAnimation = new SimpleDrawCardAnimation(hand, _simpleDrawCardsDelay);
            _fireDrawCardAnimation = new FireDrawCardAnimation(hand, _fireDrawCardsDelay);

            _startDrawCardAnimator = new StartDrawCardAnimator(new SimpleDrawCardAnimation(hand, _simpleDrawCardsDelay), _fireDrawCardAnimation);
            //_startDrawCardAnimator = new StartDrawCardAnimator(_simpleDrawCardAnimation, _fireDrawCardAnimation);
        }

        //public void StartTurnDraw(Queue<Card> cards)
        //{
        //    AddCardsInQueue(cards);

        //    StartTurnDrawing().ToUniTask();
        //}
        public void SetFireDraw(int countTurns)
        {
            _startDrawCardAnimator.SetFireMode(countTurns);
        }

        public void StartTurnDraw(Action callback = null)
        {
            _currentDrawCardAnimation = _startDrawCardAnimator;

            TakeCards(_countStartDrawCards, callback);
        }

        public List<Card> DrawCards(int countCards, Action callback = null)
        {
            _currentDrawCardAnimation = _simpleDrawCardAnimation;

            List<Card> cards = TakeCards(countCards, callback);

            return cards;
        }

        private List<Card> TakeCards(int countCards, Action callback = null)
        {
            //if (_isDrawing == false)
            //{
                List<Card> cards = new List<Card>();

                for (int i = 0; i < countCards; i++)
                {
                    if (_deck.IsHasCards(1))
                    {
                        cards.Add(_deck.TakeTopCard());
                    }
                }

                DrawingCards(cards, callback).ToUniTask();

                return cards;
            //}
            //else
            //{
            //    throw new ArgumentOutOfRangeException("Invalid draw cards timing");
            //}
        }

        //private IEnumerator StartTurnDrawing()
        //{
        //    yield return DrawingCards();

        //    _startDrawCallback?.Invoke();
        //}

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

            _isDrawing = false;

            callback?.Invoke();
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
