using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cards;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.DrawCards
{
    public class DrawCardRoot
    {
        private readonly Deck _deck;

        private IDrawCardAnimation _currentDrawCardAnimation;

        public DrawCardRoot(Deck deck)
        {
            _deck = deck;
        }

        public bool IsDrawing { get; private set; } = false;

        public void DrawCards(IDrawCardAnimation drawCardAnimation, int countCards, Action callback = null)
        {
            _currentDrawCardAnimation = drawCardAnimation;
            TakeCards(countCards, callback);
        }

        private void TakeCards(int countCards, Action callback = null)
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < countCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    cards.Add(_deck.TakeTopCard());
                }
            }

            DrawingCards(cards, callback).ToUniTask();
        }

        private IEnumerator DrawingCards(IReadOnlyList<Card> cards, Action callback)
        {
            IsDrawing = true;

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
            IsDrawing = false;
        }
    }
}