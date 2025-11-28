using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class DumbMonkEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public DumbMonkEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Глупого Монаха закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //ViewType viewType = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            ViewType viewType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            IReadOnlyList<Card> cards = _viewRoot.GetAllCards(viewType).ToList();

            if (cards.Count <= 0)
            {
                yield break;
            }

            TransitFromType transitFromType = ViewTransitTypeConverter.ConvertToTransitFromType(viewType);
            TransitToType transitToType = ViewTransitTypeConverter.GetPersonFirePoolTransitToType(_activePerson, true);

            //if (viewType == ViewType.HandPlayer)
            //{
            //    transitFromType = TransitFromType.HandPlayer;
            //    transitToType = TransitToType.PlayerFirePool;
            //}
            //else
            //{
            //    transitFromType = TransitFromType.HandEnemy;
            //    transitToType = TransitToType.EnemyFirePool;
            //}
            bool isTransit = false;
            _transitManager.TransitCard(cards[0], transitFromType, transitToType, () => isTransit = true);
            //_activePerson.ActivateFateInevitability(_duration);
            yield return new WaitUntil(() => isTransit);
        }
    }
}