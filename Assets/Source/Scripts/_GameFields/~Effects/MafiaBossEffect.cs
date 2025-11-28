using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Discovers;

namespace GameFields.Effects
{
    public class MafiaBossEffect : Effect
    {
        private const int CountViewCards = 2;
        private readonly string _activateDiscoverMessage = "Выберете, какую карту заберете";

        private readonly Person _activePerson;

        private readonly CardTransitManager _transitManager;
        private readonly CardLocationViewRoot _viewRoot;

        public MafiaBossEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _transitManager = transitManager;
            _viewRoot = viewRoot;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            //ViewType enemyhandType = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            ViewType enemyhandType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardsHand, CountViewCards, enemyhandType) == false)
            {
                yield break;
            }

            TransitFromType transitFrom = ViewTransitTypeConverter.ConvertToTransitFromType(enemyhandType);
            TransitToType transitTo = ViewTransitTypeConverter.GetPersonHandTransitToType(_activePerson, true);

            if (cardsHand.Count == 1)
            {
                bool isTransit = false;
                _transitManager.TransitCard(cardsHand[0], transitFrom, transitTo, callback: () => isTransit = true);
                yield return new WaitUntil(() => isTransit);
                yield break;
            }

            DiscoverResult discoverResult = new DiscoverResult();
            _activePerson.DiscoverCards(cardsHand, _activateDiscoverMessage, discoverResult);

            yield return new WaitUntil(() => discoverResult.Result != null);

            Card cardToTake = null;
            Card cardToFire = null;

            for (int i = 0; i < cardsHand.Count; i++)
            {
                if (cardsHand[i] == discoverResult.Result)
                {
                    cardToTake = cardsHand[i];
                }
                else
                {
                    cardToFire = cardsHand[i];
                }
            }

            _transitManager.TransitCard(cardToTake, transitFrom, transitTo);

            //TransitFromType fireFrom = _activePerson is Player ? TransitFromType.HandEnemy : TransitFromType.HandPlayer;
            //TransitToType fireTo = _activePerson is Player ? TransitToType.EnemyFirePool : TransitToType.PlayerFirePool;
            TransitFromType fireFrom = transitFrom;
            TransitToType fireTo = ViewTransitTypeConverter.GetPersonFirePoolTransitToType(_activePerson, true);
            bool isFireComplete = false;

            _transitManager.TransitCard(cardToFire, fireFrom, fireTo, () => isFireComplete = true);

            yield return new WaitUntil(() => isFireComplete);
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Босса Мафии окончен");
        }
    }
}