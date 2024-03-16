using System.Collections;
using System.Collections.Generic;
using System;
using GameFields.Persons.Hands;
using UnityEngine;
using Cards;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    public class DrawCardRoot
    {
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private float _drawCardsDelay = 2f;
        [SerializeField] private PlayerDrawCardAnimator _playerDrawCardAnimator;

        private Deck _deck;
        //private IHand _hand;
        private Transform _transform;
        private Queue<Card> _drawCardQueue;
        private bool _isDrawing;

        public void Init(Deck deck, IHand hand, Transform transform)
        {
            _deck = deck;
            //_hand = hand;
            _transform = transform;

            _isDrawing = false;
            _drawCardQueue = new Queue<Card>();

            _playerDrawCardAnimator.Init(hand, _transform);
        }

        public void StartTurnDraw()
        {
            AddCardsInQueue(_countDrawCards);

            DrawCards();
        }

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
                yield return _playerDrawCardAnimator.PlayingSimpleDrawCardAnimation(card);
            }

            _isDrawing = false;
        }

        private void AddCardsInQueue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (TryGetCard(out Card card))
                {
                    _drawCardQueue.Enqueue(card);
                }
                else
                {
                    break;
                }
            }
        }

        private bool TryGetCard(out Card card)
        {
            if (_deck.IsHasCards(1))
            {
                card = _deck.TakeTopCard();
                return true;
            }

            card = null;
            return false;
        }
    }
}
