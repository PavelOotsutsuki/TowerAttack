using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Discovers;
using GameFields.CardTransits;

namespace GameFields.Effects
{
    public class MafiaBossEffect : Effect
    {
        private const int CountViewCards = 2;
        private readonly string _activateDiscoverMessage = "Выберете, какую карту заберете";

        private readonly Person _activePerson;
        private readonly IPersonObject _deactivePerson;

        private readonly CardTransitManager _transitManager;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly ViewTransitTypesRoot _typesRoot;

        public MafiaBossEffect(IPersonObject deactivePerson, CardLocationViewRoot viewRoot,
            CardTransitManager transitManager, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;
            _transitManager = transitManager;
            _viewRoot = viewRoot;
            _typesRoot = typesRoot;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            //ViewType enemyhandType = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            //ViewType enemyhandType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);
            HandTypes deactivePersonHandTypes = _typesRoot.GetPersonTypes(_deactivePerson).Hand;
            ViewType handView = deactivePersonHandTypes.ViewType;

            PersonTypes activePersonTypes = _typesRoot.GetPersonTypes(_activePerson);

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardsHand, CountViewCards, handView) == false)
            {
                yield break;
            }

            TransitFromType handFrom = deactivePersonHandTypes.FromType;
            TransitToType handTo = activePersonTypes.Hand.ToType;

            if (cardsHand.Count == 1)
            {
                bool isTransit = false;
                _transitManager.TransitCard(cardsHand[0], handFrom, handTo, callback: () => isTransit = true);
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

            _transitManager.TransitCard(cardToTake, handFrom, handTo);

            //TransitFromType fireFrom = _activePerson is Player ? TransitFromType.HandEnemy : TransitFromType.HandPlayer;
            //TransitToType fireTo = _activePerson is Player ? TransitToType.EnemyFirePool : TransitToType.PlayerFirePool;
            TransitFromType fireHandFrom = handFrom;
            TransitToType firePoolTo = activePersonTypes.FirePool;
            int index = _viewRoot.IndexOf(handView, cardToFire);
            bool isFireComplete = false;

            _transitManager.TransitCard(cardToFire, fireHandFrom, firePoolTo, () => isFireComplete = true, index);

            yield return new WaitUntil(() => isFireComplete);
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Босса Мафии окончен");
        //}
    }
}