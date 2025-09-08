using Cards;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class TimeChildEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public TimeChildEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager, SignalBus bus, CardEffectData data)
            : base(bus, data)
        {
            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Дитя времени закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            IEnumerable<Card> firedCards = _viewRoot.GetAllCards(ViewType.FireRoot);
            Dictionary<Card, bool> isSeats = new Dictionary<Card, bool>();

            foreach (Card card in firedCards)
            {
                isSeats.Add(card, false);

                _transitManager.TransitCard(card, TransitFromType.FireRoot, TransitToType.Deck, () => isSeats.Remove(card));

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitUntil(() => isSeats.Count == 0);

            //if (_viewRoot.TryView(out IReadOnlyList<Card> cards, 1, ViewType.DiscardPile))
            //{
            //    Card card = cards[0];

            //    _transitManager.TryTransitCard(card, TransitFromType.DiscardPile, transitTo);
            //}

            yield break;
        }
    }
}