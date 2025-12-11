using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
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

        public void Play(Card card)
        {
            Playing(card).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            _isComplete = false;

            yield return new WaitForSeconds(_delay);

            _hand.SeatCard(drawnCard);
            _drawCardAdder.Add(drawnCard);
            _isComplete = true;
        }
    }
}