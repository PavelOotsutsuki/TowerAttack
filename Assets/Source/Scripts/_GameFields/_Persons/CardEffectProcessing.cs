using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons
{
    public class CardEffectProcessing : IPersonStep
    {
        private Card _currentCard;

        public bool IsComplete { get; private set; }

        public CardEffectProcessing(Card card)
        {
            IsComplete = false;
            _currentCard = card;
        }

        public void StartStep()
        {
            IsComplete = false;

            WaitingEndEffect().ToUniTask();
        }

        private IEnumerator WaitingEndEffect()
        {
            yield return new WaitUntil(() => _currentCard.IsPlayingEffect);

            IsComplete = true;
        }
    }
}
