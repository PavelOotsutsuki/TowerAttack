using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using UnityEngine;

namespace GameFields.Effects
{
    public class SharperEffect : Effect
    {
        private const string DiscoverDeckMessage = "Выберете, какую карту возьмете";
        private const string DiscoverHandMessage = "Выберете, какую карту положите на её место";

        private const int CountDeckCards = 3;
        private const int CountHandCards = 3;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public SharperEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _drawCardManager = activePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Шулера закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            ViewType hand = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;

            if (_viewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> deckCards, CountDeckCards) == false)
            {
                yield break;
            }

            if (_viewRoot.TryView(out IReadOnlyList<Card> handCards, CountHandCards, hand) == false)
            {
                yield break;
            }

            DiscoverResult deckResult = new DiscoverResult();
            _activePerson.DiscoverCards(deckCards, DiscoverDeckMessage, deckResult);

            yield return new WaitUntil(() => deckResult.IsComplete);

            Card cardFromDeck = (Card)deckResult.Result;

            DiscoverResult handResult = new DiscoverResult();
            _activePerson.DiscoverCards(handCards, DiscoverHandMessage, handResult);

            yield return new WaitUntil(() => handResult.Result != null);

            Card cardFromHand = (Card)handResult.Result;

            int indexHand = _viewRoot.IndexOf(hand, cardFromHand);
            int indexDeck = _drawCardManager.DrawCard(cardFromDeck, index: indexHand);

            TransitFromType transitFrom = _activePerson is Player ? TransitFromType.HandPlayer : TransitFromType.HandEnemy;

            _transitManager.TransitCard(cardFromHand, transitFrom, TransitToType.Deck, index: indexDeck);

            yield break;
        }
    }
}