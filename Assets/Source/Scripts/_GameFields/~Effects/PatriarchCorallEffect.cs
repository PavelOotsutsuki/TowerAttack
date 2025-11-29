using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.CardTransits;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : Effect
    {
        private readonly int _countDrawCards = 3;
        private readonly string _activateDiscoverMessage = "Выберете, какую карту отдадите противнику";

        private readonly Person _activePerson;
        private readonly IPersonObject _deactivePerson;

        private readonly CardTransitManager _transitManager;
        private readonly IDrawCardManager _drawCardManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public PatriarchCorallEffect(Person activePerson, IPersonObject deactivePerson, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

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

            //TransitFromType transitFrom = _activePerson is Player ? TransitFromType.HandPlayer : TransitFromType.HandEnemy;
            //TransitToType transitTo = _activePerson is Player ? TransitToType.HandEnemy : TransitToType.HandPlayer;
            //TransitFromType handFrom = ViewTransitTypeConverter.GetPersonHandTransitFromType(_activePerson, true);
            //TransitToType handTo = ViewTransitTypeConverter.GetPersonHandTransitToType(_activePerson, false);
            TransitFromType handFrom = _typesRoot.GetPersonTypes(_activePerson).Hand.FromType;
            TransitToType handTo = _typesRoot.GetPersonTypes(_deactivePerson).Hand.ToType;

            _transitManager.TransitCard((Card)discoverResult.Result, handFrom, handTo);

        }

        public override void End()
        {
            base.End();

            Debug.Log("End patriarch corall effect");
        }
    }
}