using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    public class DiscardCardAnimator: MonoBehaviour
    {
        private DiscardPile _discardPile;

        public void Init(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public IEnumerator DiscardCards(List<CardCharacter> discardCards)
        {
            foreach (CardCharacter cardCharacter in discardCards)
            {
                float fullDelay = 0f + 0f + 0f +2.5f;
                Card card = cardCharacter.DiscardCard();

                yield return new WaitForSeconds(fullDelay);

                _discardPile.AddCard(card);
            }
        }

        //private IEnumerator StartDiscardAnimation(Card card)
        //{

        //}
    }
}
