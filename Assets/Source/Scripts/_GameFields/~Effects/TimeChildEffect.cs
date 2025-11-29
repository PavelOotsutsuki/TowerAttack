using Cards;
using GameFields.CardTransits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Effects
{
    public class TimeChildEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public TimeChildEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Дитя времени закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            FireRootTypes fireRootTypes = _typesRoot.FireRoot;
            ViewType fireRootView = fireRootTypes.ViewType;
            IEnumerable<Card> firedCards = _viewRoot.GetAllCards(fireRootView);
            List<Card> isSeats = new List<Card>();

            foreach (Card card in firedCards)
            {
                isSeats.Add(card);

                //TransitFromType fromType = ViewTransitTypeConverter.ConvertToTransitFromType(fireType);
                TransitFromType fireRootFrom = fireRootTypes.FromType;
                _transitManager.TransitCard(card, fireRootFrom, TransitToType.Deck, () => isSeats.Remove(card));

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitUntil(() => isSeats.Count == 0);
            yield break;
        }
    }
}