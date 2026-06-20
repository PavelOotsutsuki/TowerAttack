using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CardTransits;
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
        private readonly ViewTransitTypesRoot _typesRoot;

        public SharperEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _drawCardManager = data.ActivePerson;
            _typesRoot = typesRoot;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Шулера закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            //ViewType hand = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType hand = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            HandTypes activePersonHandTypes = _typesRoot.GetPersonTypes(_activePerson).Hand;
            ViewType handView = activePersonHandTypes.ViewType;

            if (_viewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> deckCards, CountDeckCards) == false)
            {
                yield break;
            }

            if (_viewRoot.TryView(out IReadOnlyList<Card> handCards, CountHandCards, handView) == false)
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

            bool isDraw = false;
            bool isTransit = false;

            int indexHand = _viewRoot.IndexOf(handView, cardFromHand);
            int indexDeck = _drawCardManager.DrawCard(cardFromDeck, () => isDraw = true, index: indexHand);

            TransitFromType handFrom = activePersonHandTypes.FromType;

            _transitManager.TransitCard(cardFromHand, handFrom, TransitToType.Deck, () => isTransit = true, index: indexDeck);

            yield return new WaitUntil(() => isDraw && isTransit);
            yield break;
        }
    }
}