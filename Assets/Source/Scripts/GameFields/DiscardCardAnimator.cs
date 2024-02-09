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
            Vector3 position;
            Vector3 rotation;

            foreach (CardCharacter cardCharacter in discardCards)
            {
                position = _discardPile.FindCardPosition();
                rotation = _discardPile.FindCardRotation();
                cardCharacter.DiscardCard(position, rotation);
            }
        }

        //private IEnumerator StartDiscardAnimation(Card card)
        //{

        //}
    }
}
