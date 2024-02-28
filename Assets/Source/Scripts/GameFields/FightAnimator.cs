using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.Persons.Tables;
using UnityEngine;

namespace GameFields
{
    public class FightAnimator : MonoBehaviour
    {
        [SerializeField] private DiscardCardAnimator _discardCardAnimator;
        [SerializeField] private float _discardCardDelay = 0.5f;

        private DiscardPile _discardPile;

        public void Init(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        //public void StartDiscardCardAnimation(PlayedCards playedCards)
        //{
        //    foreach (CardCharacter cardCharacter in discardCards)
        //    {
        //        StartDiscardAnimation(cardCharacter).ToUniTask();
        //        yield return new WaitForSeconds(_discardCardDelay);
        //    }
        //}

        public void DiscardCards(List<Card> discardCards)
        {
            DiscardingCards(discardCards).ToUniTask();
        }

        private IEnumerator DiscardingCards(List<Card> discardCards)
        {
            foreach (Card card in discardCards)
            {
                StartDiscardCardAnimation(card).ToUniTask();
                yield return new WaitForSeconds(_discardCardDelay);
            }
        }

        private IEnumerator StartDiscardCardAnimation(Card card)
        {
            _discardCardAnimator.DiscardCard(card);
            yield return new WaitForSeconds(_discardCardAnimator.GetFullDelay());

            _discardPile.AddCard(card);
        }
    }
}
