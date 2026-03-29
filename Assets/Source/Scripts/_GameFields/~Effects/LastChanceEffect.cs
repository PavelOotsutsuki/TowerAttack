using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.CardTransits;
using System.Linq;

namespace GameFields.Effects
{
    public class LastChanceEffect : Effect
    {
        private const float StartDuration = 0.5f;

        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public LastChanceEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
             ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Последнего шанса закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            DiscardPileTypes discardPileTypes = _typesRoot.DiscardPile;
            IReadOnlyList<Card> discardPileCards = _viewRoot.GetAllCards(discardPileTypes.ViewType).ToList();

            if (discardPileCards.Count <= 0)
                yield break;

            TransitFromType discardPileFrom = discardPileTypes.FromType;
            TransitToType deckTo = _typesRoot.Deck.ToType;

            Card lastCard = discardPileCards.Last(); // Ищем последнюю карту чтобы не делать сто миллионов Shuffle Deck
            float duration = StartDuration;

            foreach (Card returnableCard in discardPileCards)
            {
                if (returnableCard != lastCard)
                {
                    _transitManager.TransitCard(returnableCard, discardPileFrom, deckTo, index: 0);
                }
                else
                {
                    _transitManager.TransitCard(returnableCard, discardPileFrom, deckTo); //Без индекса чтобы триггернуть Shuffle
                }

                yield return new WaitForSeconds(duration);
                duration = NextDuration(duration);
            }

            yield break;
        }

        private float NextDuration(float currentDuration)
        {
            return currentDuration - currentDuration / 10f;
        }
    }
}