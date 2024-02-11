using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimator: MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private DiscardPile _discardPile;

        public void Init(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public IEnumerator DiscardCards(List<CardCharacter> discardCards)
        {
            foreach (CardCharacter cardCharacter in discardCards)
            {
                StartDiscardAnimation(cardCharacter).ToUniTask();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator StartDiscardAnimation(CardCharacter cardCharacter)
        {
            float fullDelay = 0f + 0f + 0f + 2.5f;
            Card card = cardCharacter.DiscardCard();
            card.transform.SetParent(_container);

            yield return new WaitForSeconds(fullDelay);

            _discardPile.AddCard(card);
        }
    }
}
