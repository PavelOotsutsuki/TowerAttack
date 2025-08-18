using UnityEngine;
using Cards;
using GameFields.Persons.Commons;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : Effect
    {
        private readonly int _countDrawCards = 3;
        private readonly string _activateDiscoverMessage = "Выберете, какую карту отдадите противнику";

        private readonly Person _activePerson;

        private readonly CardTransitManager _transitManager;
        private readonly IDrawCardManager _drawCardManager;

        public PatriarchCorallEffect(Person activePerson, CardTransitManager transitManager) : base()
        {
            _activePerson = activePerson;
            _transitManager = transitManager;

            _drawCardManager = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            List<Card> cards = _drawCardManager?.DrawCards(_countDrawCards);

            if (cards is null)
            {
                yield break;
            }

            if (cards.Count <= 0)
            {
                yield break;
            }

            DiscoverResult discoverResult = new DiscoverResult();
            _activePerson.DiscoverCards(cards, _activateDiscoverMessage, discoverResult);

            yield return new WaitUntil(() => discoverResult.Result != null);

            TransitFromType transitFrom = _activePerson is Player ? TransitFromType.HandPlayer : TransitFromType.HandEnemy;
            TransitToType transitTo = _activePerson is Player ? TransitToType.HandEnemy : TransitToType.HandPlayer;

            _transitManager.TransitCard((Card)discoverResult.Result, transitFrom, transitTo);

            //if (_handTransitTryGet.TryGet(discoverResult.Result))
            //{
            //    _handTransitSet.Set(discoverResult.Result);
            //}
        }

        public override void End()
        {
            //Debug.Log("End patriarch corall effect");
        }

        //private void DiscoverCards()
        //{
        //    if (_cards is null)
        //    {
        //        _endPlaying = true;
        //        return;
        //    }

        //    if (_cards.Count <= 0)
        //    {
        //        _endPlaying = true;
        //        return;
        //    }

        //    DiscoverResult discoverResult = new DiscoverResult();
        //    _activePerson.DiscoverCards(_cards, _activateDiscoverMessage, discoverResult);


        //}

        //private void RechangeCards(Card card)
        //{
        //    if (_handTransitTryGet.TryGet(card))
        //    {
        //        _handTransitSet.Set(card);
        //    }

        //    _endPlaying = true;
        //}
    }
}
