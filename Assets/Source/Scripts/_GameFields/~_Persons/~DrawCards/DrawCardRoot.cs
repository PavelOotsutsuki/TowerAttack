using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Decks;

namespace GameFields.Persons.DrawCards
{
    public class DrawCardRoot : IDrawCardManager
    {
        private readonly IDrawCardAnimationWatcher _drawCardAnimationWatcher;
        private readonly IDeckTake _deck;

        private IDrawCardAnimation _currentDrawCardAnimation;

        public DrawCardRoot(IDrawCardAnimationWatcher drawCardAnimationWatcher, IDeckTake deck)
        {
            _drawCardAnimationWatcher = drawCardAnimationWatcher;
            _deck = deck;
        }

        public bool IsDrawing { get; private set; } = false;

        public List<Card> DrawCards(int countCards, Action callback = null)
        {
            _currentDrawCardAnimation = _drawCardAnimationWatcher.CurrentAnimation;
            return TakeCards(countCards, callback);
        }

        //public void DrawCards(IDrawCardAnimation drawCardAnimation, int countCards, Action callback = null)
        //{
        //    _currentDrawCardAnimation = drawCardAnimation;
        //    TakeCards(countCards, callback);
        //}

        public Card DrawCard(Card card, Action callback = null)
        {
            if (_deck.TryTakeAwayCard(card) == false)
            {
                throw new Exception("Пытаемся взять карты которой нет");
            }

            _currentDrawCardAnimation = _drawCardAnimationWatcher.CurrentAnimation;

            DrawingCards(new List<Card>() { card }, callback).ToUniTask();

            return card;
        }

        private List<Card> TakeCards(int countCards, Action callback = null)
        {
            List<Card> drawnCards = new List<Card>();
            List<Card> drawnEffectCards = new List<Card>();

            for (int i = 0; i < countCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    Card card = _deck.TakeTopCard();

                    if (_currentDrawCardAnimation is not FireDrawCardAnimation)
                        drawnCards.Add(card);

                    drawnEffectCards.Add(card);
                }
            }

            DrawingCards(drawnEffectCards, callback).ToUniTask();

            return drawnCards;
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

                    yield return new WaitUntil(() => _currentDrawCardAnimation.IsComplete);
                }
            }
            
            callback?.Invoke();
            IsDrawing = false;
            yield break;
        }
    }
}