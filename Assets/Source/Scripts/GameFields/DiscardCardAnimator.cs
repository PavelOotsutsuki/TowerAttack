using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.DiscardPiles;
using GameFields.Persons.Tables;
using UnityEngine;

namespace GameFields
{
    public class DiscardCardAnimator: MonoBehaviour
    {
        private DiscardPile _discardPile;

        public void Init(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public void DiscardCards(List<CardCharacter> discardCards)
        {
            foreach (CardCharacter cardCharacter in discardCards)
            {
                cardCharacter.DiscardCard();
                //StartDiscardAnimation(card);
            }
        }

        private void StartDiscardAnimation(Card card)
        {

        }
    }
}
