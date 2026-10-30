using System.Collections.Generic;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;

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

        public void Play(IReadOnlyList<Card> cards, int indexAdd, CancellationToken token)
        {
            Playing(cards, indexAdd, token).Forget();
        }

        private async UniTask Playing(IReadOnlyList<Card> cards, int indexAdd, CancellationToken token)
        {
            _isComplete = false;

            await UniTask.WaitForSeconds(_delay, cancellationToken: token);

            foreach (Card drawnCard in cards)
            {
                _hand.SeatCard(drawnCard, indexAdd);
                _drawCardAdder.Add(drawnCard);

                await UniTask.WaitForSeconds(_delay, cancellationToken: token);
            }

            await UniTask.WaitForSeconds(_delay, cancellationToken: token);
            _isComplete = true;
        }
    }
}