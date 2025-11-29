using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class SimpleDrawCardAnimation: IDrawCardAnimation
    {
        private readonly ICardSeatable _hand;
        private readonly float _delay;
        private readonly IDrawnCardAdder _drawCardAdder;

        private bool _isComplete;

        public SimpleDrawCardAnimation(ICardSeatable hand, IDrawnCardAdder drawCardAdder, SimpleDrawCardAnimationData data)
        {
            _hand = hand;
            _delay = data.Delay;
            _drawCardAdder = drawCardAdder;

            _isComplete = true;
        }

        public bool IsComplete => _isComplete;

        public void Play(IReadOnlyList<Card> cards, int indexAdd)
        {
            Playing(cards, indexAdd).ToUniTask();
        }

        private IEnumerator Playing(IReadOnlyList<Card> cards, int indexAdd)
        {
            _isComplete = false;

            yield return new WaitForSeconds(_delay);

            foreach (Card drawnCard in cards)
            {
                _hand.SeatCard(drawnCard, indexAdd);
                _drawCardAdder.Add(drawnCard);

                yield return new WaitForSeconds(_delay);
            }

            yield return new WaitForSeconds(_delay);
            _isComplete = true;
        }
    }
}