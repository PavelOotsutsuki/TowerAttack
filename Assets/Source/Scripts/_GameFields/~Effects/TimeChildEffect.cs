using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Effects
{
    public class TimeChildEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public TimeChildEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager, EffectData data) : base(data)
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
            ViewType fireType = ViewType.FireRoot;
            IEnumerable<Card> firedCards = _viewRoot.GetAllCards(fireType);
            List<Card> isSeats = new List<Card>();

            foreach (Card card in firedCards)
            {
                isSeats.Add(card);

                TransitFromType fromType = ViewTransitTypeConverter.ConvertToTransitFromType(fireType);
                _transitManager.TransitCard(card, fromType, TransitToType.Deck, () => isSeats.Remove(card));

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitUntil(() => isSeats.Count == 0);
            yield break;
        }
    }
}